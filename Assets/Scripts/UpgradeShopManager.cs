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
        public int basePrice;
        public int priceIncrement;
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
        gameManager = Game_manager.Instance;
        UpdateUI();

        foreach (var upgrade in upgrades)
        {
            upgrade.button.onClick.AddListener(() => PurchaseUpgrade(upgrade));
        }

        continueButton.onClick.AddListener(CloseShop);
    }

    void UpdateUI()
    {
        coinText.text = gameManager.GetCoinsCount().ToString();
        foreach (var upgrade in upgrades)
        {
            if (upgrade.level < upgrade.maxLevel)
            {
                int currentPrice = upgrade.basePrice + upgrade.priceIncrement * upgrade.level;
                upgrade.priceText.text = currentPrice.ToString();
                upgrade.levelText.text = $"Lvl: {upgrade.level}";
                upgrade.button.interactable = gameManager.GetCoinsCount() >= currentPrice;
            }
            else
            {
                upgrade.priceText.text = "Max";
                upgrade.button.interactable = false;
            }
        }
    }

    void PurchaseUpgrade(Upgrade upgrade)
    {
        int price = upgrade.basePrice + upgrade.priceIncrement * upgrade.level;
        if (gameManager.GetCoinsCount() >= price)
        {
            gameManager.coinsCount -= price;
            upgrade.level++;

            ApplyUpgradeEffect(upgrade);
            UpdateUI();
        }
    }

    void ApplyUpgradeEffect(Upgrade upgrade)
    {
        switch (upgrade.name)
        {
            case "Speed":
                Hero.Instance.speed += 1f;
                break;
            case "JumpHeight":
                Hero.Instance.jumpForce += 1f;
                break;
            case "BulletCount":
                // Дополнить при наличии логики
                break;
            case "Revive":
                // Уровень возрождения (одноразовое улучшение)
                break;
            case "CoinMultiplier":
                // Логика для множителя монет
                break;
            case "Health":
                Hero.Instance.healthPoints++;
                break;
            case "LifeTime":
                // Увеличить время жизни
                break;
        }
    }

    void CloseShop()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
