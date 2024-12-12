using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    [SerializeField] public int coinsCount = 0;
    private static Game_manager instance;

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

    public void SaveHealthData(Hero hero)
    {
        PlayerPrefs.SetInt("Hero_Health", hero.healthPoints);
        PlayerPrefs.SetInt("Hero_MaxHealth", hero.maxHealth);
        PlayerPrefs.Save();
    }

    public void LoadHealthData(Hero hero)
    {
        hero.healthPoints = PlayerPrefs.GetInt("Hero_Health", hero.healthPoints);
        hero.maxHealth = PlayerPrefs.GetInt("Hero_MaxHealth", hero.maxHealth);

        // Уведомляем HealthBar обновить количество сердечек
        HealthBar.Instance?.UpdateMaxHealth(hero.maxHealth);
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
}
