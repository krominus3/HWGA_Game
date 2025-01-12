using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    [SerializeField] public int coinsCount = 0;
    private static Game_manager instance;


    public int speedUpgrade;
    public int jumpUpgrade;
    public int healthUpgrade;
    public int lifeTimeUpgrade;
    public int coinsMultiplierUpgrade;

    public int dashUpgrade;
    public int EndGameItem;

    

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

    public void AddKillCoins(int count)
    {
        coinsCount += count * coinsMultiplierUpgrade;
    }

    public void AddCoins()
    {
        coinsCount += coinsMultiplierUpgrade;
    }

}