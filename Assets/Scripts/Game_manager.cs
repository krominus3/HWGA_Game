using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    [SerializeField] public int coinsCount = 0;
    private static Game_manager instance;

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

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

}