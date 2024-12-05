using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // ������ �� ������ ������
    public Vector2 offset;    // �������� ������ ������������ ������
    public float smoothSpeed = 0.125f; // �������� ����������� �������� ������

    private void FixedUpdate()
    {
        if (player == null) return; // ���������, �������� �� �����

        // ������������ ������� ������� ������
        Vector3 targetPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

        // ������� �������� ������ � ������� �������
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}

