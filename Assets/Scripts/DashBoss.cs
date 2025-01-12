using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoss : MonoBehaviour
{
    [SerializeField] private float flySpeed = 5f;          // �������� ������ �� �������
    [SerializeField] private float dashSpeed = 20f;       // �������� �������� ��� ����
    [SerializeField] private float dashDistance = 10f;    // ���������� ����
    [SerializeField] private float dashCooldown = 5f;     // ����� ����� ������
    [SerializeField] private float collisionPause = 0.9f;   // ����� ��������� ����� ������������
    [SerializeField] private int health = 3;
    [SerializeField] private float deathMoveSpeed = 2f;   // �������� �������� � ����� ����� ������
    [SerializeField] private Transform groundPoint;       // ����� ��� ����������� �����

    private enum BossState { Flying, Dashing, Stunned, Paused, Dying }
    private BossState currentState;

    private Transform player;
    private Vector2 dashTarget;
    private float dashTimer = 5f;
    private float pauseTimer;

    private Rigidbody2D rb;
    private Animator anim;

    private SoundManager soundManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // ��������������, ��� � ������ ��� "Player"
        rb = GetComponent<Rigidbody2D>();
        currentState = BossState.Flying;
        anim = GetComponent<Animator>();
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (!rb.simulated) return;
        //Debug.Log("������� ��������� �����: " + currentState);
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
                
                // ���� ����� �� �����, ������ �� ������
                break;
        }
    }

    private void Fly()
    {
        if (player == null) return;

        // ������ �� �������
        Vector2 targetPosition = player.position;
        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, flySpeed * Time.deltaTime);

        // ������������ ������ � ����������� ��������
        FlipModel(targetPosition);

        rb.MovePosition(newPosition);
    }

    private void PrepareDash()
    {
        if (player == null) return;

        currentState = BossState.Dashing;

        // ��������� ����������� � ������ � ���� ����
        Vector2 direction = (player.position - transform.position).normalized;
        dashTarget = (Vector2)transform.position + direction * dashDistance;

        dashTimer = dashCooldown;
    }

    private void Dash()
    {
        // ��������� � ��������� ����� � ��������� ���������
        Vector2 newPosition = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

        // ������������ ������ � ����������� ��������
        FlipModel(dashTarget);

        rb.MovePosition(newPosition);

        // ���� �������� ����� ����, ������������ � ��������� ������
        if ((Vector2)transform.position == dashTarget)
        {
            currentState = BossState.Flying;
        }
    }

    private void FlipModel(Vector2 targetPosition)
    {
        // ���������� ����������� ��������
        float direction = targetPosition.x - transform.position.x;

        // ���� �������� ������ � ������� �� X �������������, ���� ����� � �������������
        if (direction > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void Pause()
    {
        pauseTimer -= Time.deltaTime;
        if (pauseTimer <= 0)
        {
            if (health <= 0)
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
                // ������� ���� ������ � �������� ������� ������������
                Hero.Instance.GetDamage(1, transform);

                //// ����������� �����
                //Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
                //rb.AddForce(bounceDirection * 5f, ForceMode2D.Impulse);

                currentState = BossState.Flying;
            }
            else if (collision.collider.CompareTag("Projectile"))
            {
                Destroy(collision.gameObject);
            }
            else
            {
                // �������� ���� �� ������������ � ��������
                TakeDamage(1);

            }
        }
    }

    private void TakeDamage(int damage)
    {
        soundManager.PlayEnemyGetHitSound();
        health -= damage;
        print(health);

        anim.SetTrigger("getHit");
        anim.SetBool("isDashing", false);
        // ��������� � ��������� �����

        currentState = BossState.Paused;
        pauseTimer = collisionPause;


    }

    private void MoveToGroundPoint()
    {
        if (groundPoint == null) return;
        if (transform.position.x > groundPoint.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // ������ ������������ � ��������� �����
        Vector3 targetPosition = Vector3.MoveTowards(transform.position, groundPoint.position, deathMoveSpeed * Time.deltaTime);
        rb.MovePosition(targetPosition);

        // ���������������, ����� �������� �����
        if (Vector3.Distance(transform.position, groundPoint.position) < 0.1f)
        {
            
            anim.SetTrigger("isDie");
            currentState = BossState.Stunned;
            rb.velocity = Vector2.zero;
            rb.simulated = false; // ��������� ������
            Game_manager.Instance.AddKillCoins(100);
        }
    }
}
