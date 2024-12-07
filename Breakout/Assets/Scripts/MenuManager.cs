using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
    
    public GameObject levelMenu;
    public GameObject settingsMenu;

    public void ResetLevels()
    {
        PlayerPrefs.DeleteAll();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (levelMenu.activeSelf)
            {
                levelMenu.SetActive(false);
            }

            if (settingsMenu.activeSelf)
            {
                settingsMenu.SetActive(false);
            }
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        
    }
}
