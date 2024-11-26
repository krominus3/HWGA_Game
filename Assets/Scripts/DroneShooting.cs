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
        Vector2 direction = DroneController.Instance.isFacingRight ? Vector2.right : Vector2.left; // Направление по умолчанию



        if (nearestEnemy != null)
        {
            direction = (nearestEnemy.transform.position - transform.position).normalized;
        }

        // Рассчитываем угол в направлении выстрела
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Создаем снаряд и поворачиваем его в сторону выстрела
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
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
