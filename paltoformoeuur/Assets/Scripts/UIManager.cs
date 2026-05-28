using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
        Time.timeScale = 1;
        TransitionScreenOpen();

    }

    private void TransitionScreenOpen()
    {
        transitionScreen.localPosition = new Vector3(0, 0, 0);
        transitionScreen.DOLocalMove(new Vector3(-1920, 0, 0), 1)
            .IsTimeScaleIndependent();
    }

    public void MainMenu()
    {
        transitionScreen.localPosition = new Vector3(1920, 0, 0);
        transitionScreen.DOLocalMove(new Vector3(0, 0, 0), 1)
            .OnComplete((() =>
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(0);
            }));
    }
    
    
    
    
    public void Pause()
    {
        Debug.Log("2");
        if (pauseMenu == null)
        {
            Debug.Log("pas de menu poze");
            return;
        }
        
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (pauseMenu.activeSelf)
        {
            actualTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else Time.timeScale = actualTimeScale;
    }
}


