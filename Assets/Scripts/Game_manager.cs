using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    [SerializeField] public int coinsCount = 0;
    [SerializeField] public int coinMultiplier = 1;
    private static Game_manager instance;

    void Start()
    {
        LoadCoins();
        LoadCoinMultiplier();

        if (Hero.Instance != null)
        {
            LoadHeroData(Hero.Instance); // Загрузка данных героя
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public static Game_manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Game_manager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("Game_manager");
                    instance = go.AddComponent<Game_manager>();
                }
            }
            return instance;
        }
    }

    public int GetCoinsCount()
    {
        return coinsCount;
    }

    public void SaveUpgrades(UpgradeShopManager.Upgrade[] upgrades)
    {
        foreach (var upgrade in upgrades)
        {
            PlayerPrefs.SetInt($"Upgrade_{upgrade.name}_Level", upgrade.level);
        }
        PlayerPrefs.Save();
    }

    public void LoadUpgrades(UpgradeShopManager.Upgrade[] upgrades)
    {
        foreach (var upgrade in upgrades)
        {
            upgrade.level = PlayerPrefs.GetInt($"Upgrade_{upgrade.name}_Level", 0);
        }
    }

    //public void SaveHealthData(Hero hero)
    //{
    //    PlayerPrefs.SetInt("Hero_Health", hero.healthPoints);
    //    PlayerPrefs.SetInt("Hero_MaxHealth", hero.maxHealth);
    //    PlayerPrefs.Save();
    //}

    //public void LoadHealthData(Hero hero)
    //{
    //    hero.healthPoints = PlayerPrefs.GetInt("Hero_Health", hero.healthPoints);
    //    hero.maxHealth = PlayerPrefs.GetInt("Hero_MaxHealth", hero.maxHealth);

    //    // Уведомляем HealthBar обновить количество сердечек
    //    HealthBar.Instance?.UpdateMaxHealth(hero.maxHealth);
    //}

    public void SaveHeroData(Hero hero)
    {
        PlayerPrefs.SetFloat("Hero_Speed", hero.speed);
        PlayerPrefs.SetFloat("Hero_JumpForce", hero.jumpForce);
        PlayerPrefs.SetInt("Hero_Health", hero.healthPoints);
        PlayerPrefs.Save();
    }

    public void LoadHeroData(Hero hero)
    {
        hero.speed = PlayerPrefs.GetFloat("Hero_Speed", hero.speed);
        hero.jumpForce = PlayerPrefs.GetFloat("Hero_JumpForce", hero.jumpForce);
        hero.healthPoints = PlayerPrefs.GetInt("Hero_Health", hero.healthPoints);
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt("CoinsCount", coinsCount);
        PlayerPrefs.Save();
    }

    public void LoadCoins()
    {
        coinsCount = PlayerPrefs.GetInt("CoinsCount", coinsCount);
    }

    public void SaveCoinMultiplier()
    {
        PlayerPrefs.SetInt("CoinMultiplier", coinMultiplier);
        PlayerPrefs.Save();
    }

    public void LoadCoinMultiplier()
    {
        coinMultiplier = PlayerPrefs.GetInt("CoinMultiplier", 1); // По умолчанию 1
    }
}
