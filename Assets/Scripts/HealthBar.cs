using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab; // ������ ��������
    public int maxHealth; // ������������ ���������� ��
    private List<Image> hearts = new List<Image>();

    private void Start()
    {
        if (heartPrefab == null)
        {
            Debug.LogError("�� �������� ������ ��������!");
        }

        InitializeHealthBar();
    }

    private void Update()
    {
        UpdateHealthDisplay();
    }

    private void InitializeHealthBar()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heartInstance = Instantiate(heartPrefab, transform);
            hearts.Add(heartInstance.GetComponent<Image>());
        }
    }

    private void UpdateHealthDisplay()
    {
        int currentHealth = Hero.Instance.healthPoints;

        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].color = Color.white;
            }
            else
            {
                hearts[i].color = Color.gray;
            }
        }

        // ���������� ����� ��������, ���� ������� �������� ��������� ���������� ��� ������������
        while (hearts.Count < currentHealth)
        {
            GameObject heartInstance = Instantiate(heartPrefab, transform);
            hearts.Add(heartInstance.GetComponent<Image>());
        }

        // �������� ������ ��������, ���� ������� �������� ������ �� ����������
        while (hearts.Count > currentHealth)
        {
            Destroy(hearts[hearts.Count - 1].gameObject);
            hearts.RemoveAt(hearts.Count - 1);
        }
    }
}
