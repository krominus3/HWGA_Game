using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public int healthPoints;
    private List<Image> hearts = new List<Image>();

    private void Start()
    {
        if (heartPrefab == null)
        {
            Debug.LogError("Ќе назначен префаб сердечка!");
        }

        InitializeHealthBar();
    }

    private void Update()
    {
        UpdateHealthDisplay();
    }

    private void InitializeHealthBar()
    {
        for (int i = 0; i < healthPoints; i++)
        {
            GameObject heartInstance = Instantiate(heartPrefab, transform);
            hearts.Add(heartInstance.GetComponent<Image>());
        }
    }

    private void UpdateHealthDisplay()
    {
        if (heartPrefab == null)
        {
            Debug.LogError("heartPrefab is not assigned!");
            return;
        }


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

        // ƒобавление новых сердечек, если текущее здоровье превышает количество уже существующих
        while (hearts.Count < currentHealth)
        {
            GameObject heartInstance = Instantiate(heartPrefab, transform);
            hearts.Add(heartInstance.GetComponent<Image>());
        }
    }
}
