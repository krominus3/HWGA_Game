using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool PauseGame; 
    public GameObject pauseGameMenu;
    public GameObject upgradeShop;

    void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (PauseGame)
            {
                Resume(); 
            }
            else
            {
                Pause(); 
            }
        }
    }

    public void Resume()
    {
        if (upgradeShop.activeSelf)
        {
            upgradeShop.SetActive(false);
        }
        else
        {
            pauseGameMenu.SetActive(false);
        }

        Time.timeScale = 1.0f; 
        PauseGame = false;
    }

    public void Pause()
    {
        pauseGameMenu.SetActive(true); 
        Time.timeScale = 0f;           
        PauseGame = true;
    }

    public void OpenShop()
    {
        upgradeShop.SetActive(true);  
        pauseGameMenu.SetActive(false); 
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f; 
        SceneManager.LoadScene("MainMenu");
    }
}
