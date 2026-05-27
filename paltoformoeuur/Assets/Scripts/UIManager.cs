using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    private float actualTimeScale;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void Play()
    {
        if(mainMenu == null) return;
        mainMenu.SetActive(false);
    }
    
    public void Quit()
    {
        if(mainMenu == null) return;
        mainMenu.SetActive(false);
    }

    public void Pause()
    {
        if(pauseMenu == null) return;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (pauseMenu.activeSelf)
        {
            actualTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else Time.timeScale = actualTimeScale;
    }
}
