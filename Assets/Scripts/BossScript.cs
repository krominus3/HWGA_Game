using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private int damageOnWallCollision = 1;
    [SerializeField] private int health = 6;
    [SerializeField] private int damageToPlayer = 1;
    [SerializeField] private GameObject Wall;

    private Transform player;
    private Rigidbody2D rb;

    public bool isDeath = false;

    void Start()
    {
        player = Hero.Instance.transform;
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (isDeath) return;
        // Движение к игроку
        Vector2 direction = player.position - transform.position;
        direction.Normalize();
        rb.velocity += direction * acceleration * Time.deltaTime;

        // Ограничение скорости
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDeath) return;
        if ((collision.gameObject == Hero.Instance.gameObject) && (!Hero.Instance.getHit) && (!Hero.Instance.isInvulnerability))
        {
            Hero.Instance.GetDamage(damageToPlayer, rb.transform);
        }

        if (collision.gameObject == Wall)
        {
            health -= damageOnWallCollision;
            print(health + 
                "\nBoss hit the wall and took damage.");
        }

        if (health <= 0)
        {
            Boss_room.Instance.OpenDoors();
            Game_manager.Instance.coinsCount += 5;
            TimeManager.Instance.AddTime(20);
            isDeath = true;
            Destroy(this.gameObject);
        }
    }
}
