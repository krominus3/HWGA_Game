using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f; // Максимальное время жизни снаряда
    public LayerMask groundLayer; // Слой для земли

    private void Start()
    {
        // Уничтожить снаряд через lifeTime секунд
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, принадлежит ли объект слою земли
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}

