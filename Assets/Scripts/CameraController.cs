using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // Ссылка на объект игрока
    public Vector2 offset;    // Смещение камеры относительно игрока
    public float smoothSpeed = 0.125f; // Скорость сглаживания движения камеры

    private void FixedUpdate()
    {
        if (player == null) return; // Проверяем, назначен ли игрок

        // Рассчитываем целевую позицию камеры
        Vector3 targetPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

        // Плавное движение камеры к целевой позиции
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}

