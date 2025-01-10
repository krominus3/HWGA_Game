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
                    upgrade.level = gameManager.speedUpgrade; // 6 - ����������� ������� ������� �������� �����
                    break;
                case "JumpHeight":
                    upgrade.level = gameManager.jumpUpgrade; // 15 - ������� ������� ������ �����
                    break;
                case "Health":
                    upgrade.level = gameManager.healthUpgrade; // 3 - ��������� ���������� ��������
                    break;
                case "LifeTime":
                    upgrade.level = gameManager.lifeTimeUpgrade;
                    break;
                //case "Speed":
                //    upgrade.level = Mathf.Max(0, Mathf.RoundToInt(hero.speed) - 6); // 6 - ����������� ������� ������� �������� �����
                //    break;
                //case "JumpHeight":
                //    upgrade.level = Mathf.Max(0, Mathf.RoundToInt(hero.jumpForce) - 15); // 15 - ������� ������� ������ �����
                //    break;
                //case "Health":
                //    upgrade.level = Mathf.Max(0, hero.healthPoints - 3); // 3 - ��������� ���������� ��������
                //    break;
                // ��������� ������ ���������
                //case "BulletCount":
                //    // ���� ���� ������ ��� ���������� ��������, ��������, ��������� ������� 0
                //    upgrade.level = Mathf.Max(0, hero.bullets - 10); // ������ ��� ��������� ��������
                //    break;
                //case "LifeTime":
                //    // ���� ���� �������� ����� �����, ������ ��������� �������
                //    upgrade.level = Mathf.Max(0, Mathf.RoundToInt(hero.lifeTime) - 10); // ������ ����� �����
                //    break;
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
                upgrade.levelText.text = $"�������: {upgrade.level}";
                upgrade.button.interactable = gameManager.GetCoinsCount() >= currentPrice;
            }
            else
            {
                upgrade.priceText.text = "MAX";
                upgrade.levelText.text = $"�������: {upgrade.level}";
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