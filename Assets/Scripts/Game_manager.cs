using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    [SerializeField] public int coinsCount = 0;
    private static Game_manager instance;


    public int speedUpgrade = 0;
    public int jumpUpgrade = 0;
    public int healthUpgrade = 0;
    public int lifeTimeUpgrade = 0;
    public int coinsMultiplayerUpgrade = 0;

    public int dashUpgrade = 0;
    public int EndGameItem = 0;

    

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
        if (coinsMultiplayerUpgrade > 0)
        {
            coinsCount += count * 2;
        }
        else
        {
            coinsCount += count;
        }
        
    }

    public void AddCoins()
    {
        if(coinsMultiplayerUpgrade > 0)
        {
            coinsCount += 2;
        }
        else
        {
            coinsCount++;
        }
    }

}