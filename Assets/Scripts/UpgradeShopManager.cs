using UnityEngine;
using UnityEngine.UI;

public class UpgradeShopManager : MonoBehaviour
{
    [System.Serializable]
    public class Upgrade
    {
        public string name;
        public int level = 0;
        public int maxLevel = 10;
        public int basePrice = 1;
        public float growthFactor = 1.5f; // Множитель роста цены
        public Button button;
        public Text levelText;
        public Text priceText;
    }

    public Upgrade[] upgrades;
    public Text coinText;
    public Button continueButton;
    private Game_manager gameManager;

    void Start()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        gameManager = Game_manager.Instance;

        UpdateUI();

        InitializeUpgrades();

        foreach (var upgrade in upgrades)
        {
            var capturedUpgrade = upgrade;
            upgrade.button.onClick.AddListener(() => PurchaseUpgrade(capturedUpgrade));
        }

        continueButton.onClick.AddListener(CloseShop);
    }

    private void Update()
    {
        UpdateUI();
    }

    void InitializeUpgrades()
    {
        foreach (var upgrade in upgrades)
        {
            switch (upgrade.name)
            {
                case "Speed":
                    upgrade.level = gameManager.speedUpgrade;
                    break;
                case "JumpHeight":
                    upgrade.level = gameManager.jumpUpgrade;
                    break;
                case "Health":
                    upgrade.level = gameManager.healthUpgrade;
                    break;
                case "LifeTime":
                    upgrade.level = gameManager.lifeTimeUpgrade;
                    break;
                case "CoinsMultiplayer":
                    upgrade.level = gameManager.coinsMultiplayerUpgrade;
                    break;
                case "Dash":
                    upgrade.level = gameManager.dashUpgrade;
                    break;
                case "EndGameItem":
                    upgrade.level = gameManager.EndGameItem;
                    break;
                default:
                    Debug.LogWarning($"Неизвестное улучшение: {upgrade.name}");
                    break;
            }

            upgrade.level = Mathf.Clamp(upgrade.level, 0, upgrade.maxLevel);
        }
    }

    void UpdateUI()
    {
        coinText.text = gameManager.GetCoinsCount().ToString();
        foreach (var upgrade in upgrades)
        {
            if (upgrade.level < upgrade.maxLevel)
            {
                int currentPrice = Mathf.RoundToInt(upgrade.basePrice * Mathf.Pow(upgrade.growthFactor, upgrade.level));
                upgrade.priceText.text = currentPrice.ToString();
                upgrade.levelText.text = $"Уровень: {upgrade.level}";
                upgrade.button.interactable = gameManager.GetCoinsCount() >= currentPrice;
            }
            else
            {
                upgrade.priceText.text = "MAX";
                upgrade.levelText.text = $"Уровень: {upgrade.level}";
                upgrade.button.interactable = false;
            }
        }
    }

    void PurchaseUpgrade(Upgrade upgrade)
    {
        int price = Mathf.RoundToInt(upgrade.basePrice * Mathf.Pow(upgrade.growthFactor, upgrade.level));
        if (gameManager.GetCoinsCount() >= price && upgrade.level < upgrade.maxLevel)
        {
            gameManager.coinsCount -= price;
            upgrade.level++;

            ApplyUpgradeEffect(upgrade);
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins or max level reached!");
        }
    }

    void ApplyUpgradeEffect(Upgrade upgrade)
    {
        switch (upgrade.name)
        {
            case "Speed":
                gameManager.speedUpgrade += 1;
                break;
            case "JumpHeight":
                gameManager.jumpUpgrade += 1;
                break;
            case "Health":
                gameManager.healthUpgrade += 1;
                break;
            case "LifeTime":
                gameManager.lifeTimeUpgrade += 1;
                break;
            case "CoinsMultiplayer":
                gameManager.coinsMultiplayerUpgrade++;
                break;
            case "Dash":
                gameManager.dashUpgrade++;
                break;
            case "EndGameItem":
                gameManager.EndGameItem++;
                break;
        }
    }

    void CloseShop()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
