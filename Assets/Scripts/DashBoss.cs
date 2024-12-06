using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoss : MonoBehaviour
{
    public float flySpeed = 5f;          // Скорость полета за игроком
    public float dashSpeed = 20f;       // Скорость движения при дэше
    public float dashDistance = 10f;    // Расстояние дэша
    public float dashCooldown = 5f;     // Время между дэшами
    public float collisionPause = 0.9f;   // Время остановки после столкновения
    public float deathMoveSpeed = 2f;   // Скорость движения в точку после смерти
    public int maxHealth = 3;           // Максимальное количество здоровья
    public Transform groundPoint;       // Точка для приземления босса

    private enum BossState { Flying, Dashing, Stunned, Paused, Dying }
    private BossState currentState;

    private Transform player;
    private Vector2 dashTarget;
    private int currentHealth;
    private float dashTimer = 5f;
    private float pauseTimer;

    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Предполагается, что у игрока тег "Player"
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        currentState = BossState.Flying;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!rb.simulated) return;
        //Debug.Log("Текущее состояние босса: " + currentState);
        switch (currentState)
        {
            case BossState.Flying:
                anim.SetBool("isDashing", false);
                Fly();
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0)
                {
                    PrepareDash();
                }
                break;

            case BossState.Dashing:
                anim.SetBool("isDashing", true);
                Dash();
                break;

            case BossState.Paused:
                
                Pause();
                break;

            case BossState.Dying:
                MoveToGroundPoint();
                break;

            case BossState.Stunned:
                
                // Босс стоит на месте, ничего не делает
                break;
        }
    }

    private void Fly()
    {
        if (player == null) return;

        // Лететь за игроком
        Vector2 targetPosition = player.position;
        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, flySpeed * Time.deltaTime);
        rb.MovePosition(newPosition);
    }

    private void PrepareDash()
    {
        if (player == null) return;

        currentState = BossState.Dashing;

        // Вычисляем направление к игроку и цель дэша
        Vector2 direction = (player.position - transform.position).normalized;
        dashTarget = (Vector2)transform.position + direction * dashDistance;

        dashTimer = dashCooldown;
    }

    private void Dash()
    {
        // Двигаемся к расчетной точке с указанной скоростью
        Vector2 newPosition = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);

        // Если достигли точки дэша, возвращаемся в состояние полета
        if ((Vector2)transform.position == dashTarget)
        {
            currentState = BossState.Flying;
        }
    }

    private void Pause()
    {
        pauseTimer -= Time.deltaTime;
        if (pauseTimer <= 0)
        {
            if (currentHealth <= 0)
            {
                currentState = BossState.Dying;
            }
            else
            {
                currentState = BossState.Flying;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState != BossState.Stunned)
        {
            if (collision.collider.CompareTag("Player"))
            {
                // Наносим урон игроку и передаем позицию столкновения
                Hero.Instance.GetDamage(1, transform);

                //// Отскакиваем назад
                //Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
                //rb.AddForce(bounceDirection * 5f, ForceMode2D.Impulse);

                currentState = BossState.Flying;
            }
            else
            {
                // Получаем урон от столкновения с объектом
                TakeDamage(1);

            }
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        print(currentHealth);

        anim.SetTrigger("getHit");
        anim.SetBool("isDashing", false);
        // Переходим в состояние паузы

        currentState = BossState.Paused;
        pauseTimer = collisionPause;


    }

    private void MoveToGroundPoint()
    {
        if (groundPoint == null) return;

        // Плавно перемещаемся в указанную точку
        Vector3 targetPosition = Vector3.MoveTowards(transform.position, groundPoint.position, deathMoveSpeed * Time.deltaTime);
        rb.MovePosition(targetPosition);

        // Останавливаемся, когда достигли точки
        if (Vector3.Distance(transform.position, groundPoint.position) < 0.1f)
        {
            
            anim.SetTrigger("isDie");
            currentState = BossState.Stunned;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true; // Отключаем физику
        }
    }
}
