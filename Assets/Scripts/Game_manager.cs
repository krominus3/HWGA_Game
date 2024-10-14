using UnityEngine;

public class Game_manager : MonoBehaviour
{
    private static Game_manager _instance;

    public static Game_manager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Game_manager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "Game_manager";
                    _instance = go.AddComponent<Game_manager>();
                }
            }
            return _instance;
        }
    }

    public int coinsCount = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
