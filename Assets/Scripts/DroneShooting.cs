using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneShooting : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab; // ������ �������
    [SerializeField] Transform firePoint; // �����, ������ �������� ������
    [SerializeField] float projectileSpeed = 10f; // �������� �������
    [SerializeField] float fireCooldown = 1f; // �������� ����� ����������
    [SerializeField] LayerMask enemyLayer; // ���� ������
    [SerializeField] float autoAimRadius = 5f; // ������ �������������
    
    private float fireTimer;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F) && fireTimer >= fireCooldown)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        // ����� ���������� ����� � �������
        Collider2D nearestEnemy = Physics2D.OverlapCircle(transform.position, autoAimRadius, enemyLayer);
        Vector2 direction = Vector2.right; // ����������� �� ���������

        if (nearestEnemy != null)
        {
            direction = (nearestEnemy.transform.position - transform.position).normalized;
        }

        // ������� ������
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;

        // ����� �������� ������/���� ��������
    }

    // ��� ������������ ������� ������������� � ���������
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, autoAimRadius);
    }
}
