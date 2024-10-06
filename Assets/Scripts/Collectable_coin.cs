using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        Game_manager player = collision.gameObject.GetComponent<Game_manager>();
        if (player)
        {
            player.coinsCount++;
            Destroy(this.gameObject);
        }
    }
}
