using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    
    public void GotoMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void GotoEndGameScene()
    {
        SceneManager.LoadScene(2);
    }
}