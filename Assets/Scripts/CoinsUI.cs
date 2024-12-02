using UnityEngine;
using UnityEngine.UI;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private Text coinsText; 

    [SerializeField] private Game_manager coinManager; 

    private void Start()
    {
        if (coinManager != null)
        {
            UpdateCoinsUI();
        }
        else
        {
            Debug.LogError("CoinsUI: Coin Manager не установлен в инспекторе.");
        }
    }

    private void Update()
    {
        if (coinManager != null)
        {
            UpdateCoinsUI();
        }
    }

    private void UpdateCoinsUI()
    {
        int currentCoins = coinManager.GetCoinsCount(); 
        coinsText.text = currentCoins.ToString();
    }
}