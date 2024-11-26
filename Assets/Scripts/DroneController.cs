using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class DroneController : MonoBehaviour
{
    [SerializeField] Transform player; // ������ �� ������
    [SerializeField] float hoverHeight = 2.8f; // ������ ��� �������
    [SerializeField] float followSpeed = 2f; // �������� ����������
    [SerializeField] float hoverAmplitude = 0.3f; // ��������� �����������
    [SerializeField] float hoverFrequency = 2f; // ������� �����������
    
    private Vector3 offset; // �������� ������������ ������
    private float hoverTimer; // ����� ��� ��������������� ��������
    private bool isFacingRight = true;
    private Vector3 pos, velocity;

    void Start()
    {
        offset = new Vector3(0, hoverHeight, 0); // ������ ��������� ��������
        pos = transform.position;
    }

    void Update()
    {
        if (player == null) return;
        Flip();

        // �������� � �������������
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

