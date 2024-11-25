using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float attackRange = 5f;
    [SerializeField] float followDistance = 1f;
    [SerializeField] GameObject projectilePrefab; // ������ ��� ������� (�����������)
    [SerializeField] float projectileSpeed = 10f;
    
    private Rigidbody2D rb;
    private GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); // ������ ������ �� ����
        }
    }

    void Update()
    {
        FollowPlayer();

        if (Input.GetKeyDown(KeyCode.F)) // ����� �� f
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
            Vector2 perpendicularDirection = Vector3.Cross(direction, Vector3.forward).normalized; // 2D - ���������� Vector3.forward ��� ������ "�����"

            //����������� �������� (������ ����������� ��������)
            float rotationDirection = 1f; // ��� -1f ��� �������� � ������ �������.

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
                // �������� ��������
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            }
            else
            {
                // ������ ����� (��������, ��������� ����� �����)
                Debug.Log("Attacking " + enemy.name);
                // ����� �������� ������ ��������� ����� �����
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
