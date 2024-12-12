using System.Collections.Generic;
using UnityEngine;

public class Boss_room : MonoBehaviour
{
    [SerializeField] List<Animator> doorAnimators;
    [SerializeField] Rigidbody2D bossRB;

    private BoxCollider2D bc;
    private bool doorsClosed = false; // ���� ��� ������������ ��������� ������

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        bossRB.simulated = false; // ������������, ��� Rigidbody2D ����� �������� �� ���������
    }

    private void FixedUpdate()
    {
        if (bossRB.simulated)
        {
            CloseDoors();
        }
        else
        {
            OpenDoors();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bc.enabled = false;
            bossRB.simulated = true;
            CloseDoors();
        }
    }

    private void CloseDoors()
    {
        if (!doorsClosed) // ���������, ������� �� ��� �����
        {
            UpdateDoors("close");
            doorsClosed = true;
        }
    }

    public void OpenDoors()
    {
        if (doorsClosed) // ���������, ������� �� ��� �����
        {
            UpdateDoors("open");
            doorsClosed = false;
        }
    }

    public void UpdateDoors(string command)
    {
        foreach (Animator anim in doorAnimators)
        {
            anim.SetTrigger(command);
        }
    }
}
