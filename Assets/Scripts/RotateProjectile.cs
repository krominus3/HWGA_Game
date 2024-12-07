using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingProjectile : Projectile
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // ���� �������� ����, ��������� ���� ��������
        if (rb != null && rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������, ���� �� � ������� ����� GetDamage
        var damageable = collision.gameObject.GetComponent<Hero>();
        if (damageable != null)
        {
            // �������� ����� GetDamage
            damageable.GetDamage(1, transform); // ��������� 10 ����� ��� ������
        }

        // ���������� ������ ����� ��������� �����
        Destroy(gameObject);
    }
}
