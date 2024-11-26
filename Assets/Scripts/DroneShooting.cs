using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneShooting : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab; // Префаб снаряда
    [SerializeField] Transform firePoint; // Точка, откуда вылетает снаряд
    [SerializeField] float projectileSpeed = 10f; // Скорость снаряда
    [SerializeField] float fireCooldown = 1f; // Задержка между выстрелами
    [SerializeField] LayerMask enemyLayer; // Слой врагов
    [SerializeField] float autoAimRadius = 5f; // Радиус автонаведения
    
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
        // Найти ближайшего врага в радиусе
        Collider2D nearestEnemy = Physics2D.OverlapCircle(transform.position, autoAimRadius, enemyLayer);
        Vector2 direction = Vector2.right; // Направление по умолчанию

        if (nearestEnemy != null)
        {
            direction = (nearestEnemy.transform.position - transform.position).normalized;
        }

        // Создаем снаряд
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;

        // Можно добавить эффект/звук выстрела
    }

    // Для визуализации радиуса автонаведения в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, autoAimRadius);
    }
}
