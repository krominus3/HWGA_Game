using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class DroneController : MonoBehaviour
{
    [SerializeField] Transform player; // Ссылка на игрока
    [SerializeField] float hoverHeight = 2.8f; // Высота над игроком
    [SerializeField] float followSpeed = 2f; // Скорость следования
    [SerializeField] float hoverAmplitude = 0.3f; // Амплитуда покачиваний
    [SerializeField] float hoverFrequency = 2f; // Частота покачиваний
    
    private Vector3 offset; // Смещение относительно игрока
    private float hoverTimer; // Время для синусоидального движения
    private bool isFacingRight = true;
    private Vector3 pos, velocity;

    void Start()
    {
        offset = new Vector3(0, hoverHeight, 0); // Задаем начальное смещение
        pos = transform.position;
    }

    void Update()
    {
        if (player == null) return;
        Flip();

        // Движение с покачиваниями
        hoverTimer += Time.deltaTime * hoverFrequency;
        float hoverOffset = Mathf.Sin(hoverTimer) * hoverAmplitude;
        Vector3 targetPosition = player.position + offset + new Vector3(0, hoverOffset, 0);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    private void Flip()
    {
        velocity = (transform.position - pos) / Time.deltaTime;
        pos = transform.position;

        if (isFacingRight && velocity.x < 0f || !isFacingRight && velocity.x > 0f)
        {
            Vector2 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}

