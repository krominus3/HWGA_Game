using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelRestart : MonoBehaviour
{
    void Update()
    {
        // ���������� ������ ��� ������� ������� R
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������� ������ ��� ������������ � ���������
        RestartLevel();
    }

    public void RestartLevel()
    {
        if (Hero.Instance != null)
        {
            Hero.Instance.ResetHealth(); // ����� �������� �����
            HealthBar.Instance?.UpdateMaxHealth(Hero.Instance.maxHealth); // ���������, ��� ������������ ���������� ���������� ��������
        }

        // ���������� ������
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

}