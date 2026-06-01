using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
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
        transitionScreen.gameObject.SetActive(true);
        if(pauseMenu != null) pauseMenu.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine(TransitionOpen());
    }
    

    private IEnumerator TransitionOpen()
    {
        yield return new WaitForSeconds(0.5f);
        transitionScreen.localPosition = new Vector3(0, 0, 0);
        transitionScreen.DOLocalMove(new Vector3(-1920, 0, 0), 1).SetUpdate(true);
    }
    

    public void LoadScene(int scene)
    {
        transitionScreen.localPosition = new Vector3(1920, 0, 0);
        transitionScreen.DOLocalMove(new Vector3(0, 0, 0), 1)
            .SetUpdate(true)
            .OnComplete((() =>
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(scene);
            }));
    }


    
    
    public void Quit()
    {
        Application.Quit();
        return;
    }
    
    public void Pause()
    {
        Debug.Log(gameObject.name);
        if (pauseMenu == null)
        {
            Debug.Log("pas de menu poze");
            return;
        }

        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (pauseMenu.activeSelf)
        {
            PlayerManager.instance.PlayerInput.SwitchCurrentActionMap("UI");
            Debug.Log(PlayerManager.instance.PlayerInput.currentActionMap);
            actualTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            PlayerManager.instance.PlayerInput.SwitchCurrentActionMap("Player");
            Debug.Log(PlayerManager.instance.PlayerInput.currentActionMap);
            Time.timeScale = actualTimeScale;
        }
    }
}


