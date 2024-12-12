using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance { get; private set; } // ��������

    public GameObject heartPrefab; // ������ ��������
    public int maxHealth; // ������������ ���������� ��
    private List<Image> hearts = new List<Image>();

    private void Awake()
    {
        // ���������� ���������
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public void ResetHealthBar()
    {
        // ������� ��� ������� ��������
        foreach (var heart in hearts)
        {
            Destroy(heart.gameObject);
        }
        hearts.Clear();

        // ������ ����� �������� � ������������ � maxHealth
        InitializeHealthBar();
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
    }

    public void UpdateMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        ResetHealthBar();
    }

}