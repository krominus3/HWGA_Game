using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame; // Флаг паузы игры
    public GameObject pauseGameMenu; // Панель меню паузы
    public GameObject upgradeShop; // Панель магазина

    void Update()
    {
        // Обработка клавиши Escape для открытия/закрытия меню паузы
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (PauseGame)
            {
                Resume(); // Снять паузу или закрыть магазин
            }
            else
            {
                Pause(); // Открыть меню паузы
            }
        }
    }

    // Метод для снятия паузы
    public void Resume()
    {
        // Если открыт магазин, закрываем его
        if (upgradeShop.activeSelf)
        {
            upgradeShop.SetActive(false);
        }
        else
        {
            // Иначе закрываем меню паузы
            pauseGameMenu.SetActive(false);
        }

        Time.timeScale = 1.0f; // Снимаем паузу
        PauseGame = false;
    }

    // Метод для постановки игры на паузу
    public void Pause()
    {
        pauseGameMenu.SetActive(true); // Показываем меню паузы
        Time.timeScale = 0f;           // Ставим игру на паузу
        PauseGame = true;
    }

    // Метод для открытия магазина
    public void OpenShop()
    {
        upgradeShop.SetActive(true);  // Показываем панель магазина
        pauseGameMenu.SetActive(false); // Закрываем меню паузы
    }

    // Метод для возврата в главное меню
    public void LoadMenu()
    {
        Time.timeScale = 1.0f; // Снимаем паузу
        SceneManager.LoadScene("MainMenu"); // Загружаем сцену главного меню
    }
}
