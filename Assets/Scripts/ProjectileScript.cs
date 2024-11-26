using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f; // ������������ ����� ����� �������
    public LayerMask groundLayer; // ���� ��� �����

    private void Start()
    {
        // ���������� ������ ����� lifeTime ������
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������, ����������� �� ������ ���� �����
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}

