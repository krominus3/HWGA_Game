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
    private SoundManager soundManager;

    private bool pikedUp = false;


    private void Start()
    {
        game_Manager = Game_manager.Instance; // �������� ������ �� Game_manager
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();    
        anim = GetComponent<Animator>();
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
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
            //��������� ����
            TimeManager.Instance.AddTime(5);
            //� ��������� ���������� ����������� ����� ������� �������
            soundManager.PlayCoinSound();
        }
    }

    private void destroyObject()
    {
        Destroy(this.gameObject);
    }


}