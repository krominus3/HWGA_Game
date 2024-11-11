using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_coin : MonoBehaviour
{
    private Game_manager game_Manager;

    private void Start()
    {
        game_Manager = Game_manager.Instance; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Hero")
        {
            game_Manager.coinsCount++;
            Destroy(this.gameObject);
        }
    }
}