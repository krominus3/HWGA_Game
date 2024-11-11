using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Hero hero; 
    public Image[] hearts; 

    private void Start()
    {
        if (hero == null)
        {
            hero = Hero.Instance; 
        }

        if (hearts.Length == 0)
        {
            Debug.LogError("Не назначены сердечка в HealthBar!");
        }
    }

    private void Update()
    {
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        int currentHealth = hero.healthPoints;

        for (int i = 0; i < hearts.Length; i++)
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
}

