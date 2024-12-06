using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroneShooting : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab; // ������ �������
    [SerializeField] Transform firePoint; // �����, ������ �������� ������
    [SerializeField] float projectileSpeed = 10f; // �������� �������
    [SerializeField] float fireCooldown = 1f; // �������� ����� ����������
    [SerializeField] LayerMask enemyLayer; // ���� ������
    [SerializeField] float autoAimRadius = 5f; // ������ �������������

    SoundManager soundManager;
    
    private float fireTimer;

    private void Awake()
    {
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F) && fireTimer >= fireCooldown)
        {
            fireTimer = 0f;
            Shoot();
            soundManager.PlayShotSound();
        }
    }

    void Shoot()
    {

        // ����� ���������� ����� � �������
        Collider2D nearestEnemy = Physics2D.OverlapCircle(transform.position, autoAimRadius, enemyLayer);
        Vector2 direction = DroneController.Instance.isFacingRight ? Vector2.right : Vector2.left; // ����������� �� ���������



        if (nearestEnemy != null)
        {
            direction = (nearestEnemy.transform.position - transform.position).normalized;
        }

        // ������������ ���� � ����������� ��������
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ������� ������ � ������������ ��� � ������� ��������
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
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
