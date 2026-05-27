using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private RectTransform transitionScreen;
    private float actualTimeScale;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        transitionScreen.DOLocalMove(new Vector3(-1920, 0, 0), 2)
            .OnComplete((() =>
            {
                transitionScreen.localPosition = new Vector3(1920, 0, 0);
            }));
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


