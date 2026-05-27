using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;


    [SerializeField] private GameObject pauseMenu;
    private float actualTimeScale;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }


    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    
    public void Pause()
    {
        if (pauseMenu == null) return;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (pauseMenu.activeSelf)
        {
            actualTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else Time.timeScale = actualTimeScale;
    }
}


