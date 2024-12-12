using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelRestart : MonoBehaviour
{
    void Update()
    {
        // Перезапуск уровня при нажатии клавиши R
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Перезапуск уровня при столкновении с триггером
        RestartLevel();
    }

    public void RestartLevel()
    {
        if (Hero.Instance != null)
        {
            Hero.Instance.ResetHealth(); // Сброс здоровья героя
            HealthBar.Instance?.UpdateMaxHealth(Hero.Instance.maxHealth); // Убедитесь, что отображается правильное количество сердечек
        }

        // Перезапуск уровня
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

}