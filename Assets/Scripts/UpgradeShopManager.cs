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

    void Start()
    {
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
                    upgrade.level = gameManager.speedUpgrade; // 6 - минимальный базовый уровень скорости героя
                    break;
                case "JumpHeight":
                    upgrade.level = gameManager.jumpUpgrade; // 15 - базовый уровень прыжка героя
                    break;
                case "Health":
                    upgrade.level = gameManager.healthUpgrade; // 3 - начальное количество здоровья
                    break;
                case "LifeTime":
                    upgrade.level = gameManager.lifeTimeUpgrade;
                    break;
                //case "Speed":
                //    upgrade.level = Mathf.Max(0, Mathf.RoundToInt(hero.speed) - 6); // 6 - минимальный базовый уровень скорости героя
                //    break;
                //case "JumpHeight":
                //    upgrade.level = Mathf.Max(0, Mathf.RoundToInt(hero.jumpForce) - 15); // 15 - базовый уровень прыжка героя
                //    break;
                //case "Health":
                //    upgrade.level = Mathf.Max(0, hero.healthPoints - 3); // 3 - начальное количество здоровья
                //    break;
                // Возможные другие улучшения
                //case "BulletCount":
                //    // Если есть логика для количества патронов, например, начальный уровень 0
                //    upgrade.level = Mathf.Max(0, hero.bullets - 10); // Пример для начальных патронов
                //    break;
                //case "LifeTime":
                //    // Если есть параметр жизни героя, задать начальный уровень
                //    upgrade.level = Mathf.Max(0, Mathf.RoundToInt(hero.lifeTime) - 10); // Пример жизни героя
                //    break;
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
                int currentPrice = upgrade.basePrice + upgrade.priceIncrement * upgrade.level;
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

            //case "Speed":
            //    hero.speed += 1f;
            //    break;
            //case "JumpHeight":
            //    hero.jumpForce += 1f;
            //    break;
            //case "Health":
            //    hero.healthPoints++;
            //    break;

        }
    }

    void CloseShop()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}