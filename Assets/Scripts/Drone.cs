using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float attackRange = 5f;
    [SerializeField] float followDistance = 1f;
    [SerializeField] GameObject projectilePrefab; // Префаб для снаряда (опционально)
    [SerializeField] float projectileSpeed = 10f;
    
    private Rigidbody2D rb;
    private GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); // Найдем игрока по тегу
        }
    }

    void Update()
    {
        FollowPlayer();

        if (Input.GetKeyDown(KeyCode.F)) // Атака по f
        {
            Attack();
        }
    }

    void FollowPlayer()
    {
        if (player == null) return;

        Vector3 playerPosition = player.transform.position + new Vector3(0f, 3f);
        Vector2 direction = (playerPosition - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        if (distanceToPlayer > followDistance)
        {
            rb.velocity = direction * speed;
        }
        else
        {
            Vector2 perpendicularDirection = Vector3.Cross(direction, Vector3.forward).normalized; // 2D - используем Vector3.forward как вектор "вверх"

            //Направление вращения (меняет направление вращения)
            float rotationDirection = 1f; // или -1f для вращения в другую сторону.

            rb.velocity = perpendicularDirection * rotationSpeed * rotationDirection;
        }
    }


    void Attack()
    {
        GameObject enemy = FindNearestEnemy();
        if (enemy != null)
        {
            if (projectilePrefab != null)
            {
                // Стрельба снарядом
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            }
            else
            {
                // Прямая атака (например, нанесение урона врагу)
                Debug.Log("Attacking " + enemy.name);
                // Здесь добавьте логику нанесения урона врагу
            }
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && distance < attackRange)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }
}
