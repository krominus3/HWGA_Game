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
        public int priceIncrement = 1;
        public Button button;
        public Text levelText;
        public Text priceText;
    }

    public Upgrade[] upgrades;
    public Text coinText;
    public Button continueButton;
    private Game_manager gameManager;
    private Hero hero;

    void Start()
    {
        gameManager = Game_manager.Instance;

        if (Hero.Instance != null)
        {
            hero = Hero.Instance;
        }
        else
        {
            Debug.LogError("Hero instance not found!");
            return;
        }

        gameManager.LoadUpgrades(upgrades); // �������� ���������
        gameManager.LoadCoins();           // �������� �����
        gameManager.LoadCoinMultiplier();  // �������� ��������� �����
        UpdateUI();
        InitializeUpgrades();

        foreach (var upgrade in upgrades)
        {
            var capturedUpgrade = upgrade;
            upgrade.button.onClick.AddListener(() => PurchaseUpgrade(capturedUpgrade));
        }

        continueButton.onClick.AddListener(CloseShop);
    }


    void InitializeUpgrades()
    {
        foreach (var upgrade in upgrades)
        {
            switch (upgrade.name)
            {
                case "Speed":
                    upgrade.level = Mathf.Max(0, Mathf.RoundToInt(hero.speed) - 6); // 6 - ����������� ������� ������� �������� �����
                    break;
                case "JumpHeight":
                    upgrade.level = Mathf.Max(0, Mathf.RoundToInt(hero.jumpForce) - 15); // 15 - ������� ������� ������ �����
                    break;
                case "Health":
                    upgrade.level = Mathf.Max(0, hero.healthPoints - 3); // 3 - ��������� ���������� ��������
                    break;
                case "CoinMultiplier":
                    upgrade.level = Mathf.Max(0, gameManager.coinMultiplier - 1); // 1 - ������� ���������
                    break;
                default:
                    Debug.LogWarning($"����������� ���������: {upgrade.name}");
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
                int currentPrice = upgrade.basePrice + upgrade.priceIncrement * upgrade.level;
                upgrade.priceText.text = currentPrice.ToString();
                upgrade.levelText.text = $"�������: {upgrade.level}/{upgrade.maxLevel}";
                upgrade.button.interactable = gameManager.GetCoinsCount() >= currentPrice;
            }
            else
            {
                upgrade.priceText.text = "MAX";
                upgrade.levelText.text = $"�������: {upgrade.level}/{upgrade.maxLevel}";
                upgrade.button.interactable = false;
            }
        }
    }

    void PurchaseUpgrade(Upgrade upgrade)
    {
        int price = upgrade.basePrice + upgrade.priceIncrement * upgrade.level;
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
                hero.speed += 1f;
                break;
            case "JumpHeight":
                hero.jumpForce += 1f;
                break;
            case "Health":
                hero.healthPoints++;
                break;
            case "CoinMultiplier":
                gameManager.coinMultiplier++;
                gameManager.SaveCoinMultiplier(); // ���������� ���������

                break;

        }
    }

    void OnEnable()
    {
        UpdateUI();
    }

    void CloseShop()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
