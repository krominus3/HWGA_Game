using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Collectable_coin : MonoBehaviour
{
    private Game_manager game_Manager;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    private bool pikedUp = false;

    private void Start()
    {
        game_Manager = Game_manager.Instance; // Получаем ссылку на Game_manager
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    
        anim = GetComponent<Animator>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Hero" && !pikedUp)
        {
            game_Manager.coinsCount++;
            pikedUp = true;
            anim.SetTrigger("pickUp");
            rb.gravityScale = 1;
            rb.velocity = new Vector2(1, 2);
            
            //в аниматоре вызывается уничтожение после анмации подбора

        }
    }

    private void destroyObject()
    {
        Destroy(this.gameObject);
    }


}