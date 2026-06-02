using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private enum Menu
    {
        noMenu,
        pause,
        settings,
    }
    
    [Header("menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject defaultPauseMenu;
    [SerializeField] private GameObject settingsMenu;
    
    [Header("settings menu")]
        [SerializeField] private GameObject[] settingsTab;
        [SerializeField] private GameObject[] settingsTabIcon;
        private int settingsTabIndex = 0;
    

    [Header("transition")]
    [SerializeField] private RectTransform transitionScreen;
    
    
    private float actualTimeScale;
    private Menu menu;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        PlayerManager.instance.PlayerInput.SwitchCurrentActionMap("Player");
        ChangeMenu(Menu.noMenu);
        
        
        
        transitionScreen.gameObject.SetActive(true);
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


    private void ChangeMenu(Menu newMenu)
    {
        switch (newMenu)
        {
            case(Menu.noMenu):
                PlayerManager.instance.PlayerInput.SwitchCurrentActionMap("Player");
                Time.timeScale = actualTimeScale;
                pauseMenu.SetActive(false);
                defaultPauseMenu.SetActive(false);
                settingsMenu.SetActive(false);
                break;
            
            case(Menu.pause):
                PlayerManager.instance.PlayerInput.SwitchCurrentActionMap("UI");
                actualTimeScale = Time.timeScale;
                Time.timeScale = 0;
                settingsTabIndex = 0;
                
                pauseMenu.SetActive(true);
                defaultPauseMenu.SetActive(true);
                settingsMenu.SetActive(false);
                break;
            
            case(Menu.settings):
                pauseMenu.SetActive(true);
                defaultPauseMenu.SetActive(false);
                settingsMenu.SetActive(true);
                UpdateSettingsTab();
                break;
        }
    }
    
    
    
    
    
    //pour input manette
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Pause();
        }
    }

    //pour bouton
    public void Pause()
    {
        if (pauseMenu == null) return;
       
        if (menu == Menu.noMenu)
        {
            ChangeMenu(Menu.pause);
        }
        else
        {
            ChangeMenu(Menu.noMenu);
        }
        
    }
    public void Settings()
    {
        ChangeMenu(Menu.settings);
        UpdateSettingsTab();
    }
    

    public void NextTab(InputAction.CallbackContext context)
    {
        if (menu == Menu.settings)
        {
            if (context.started)
            {
                settingsTabIndex = (settingsTabIndex + 1) % settingsTab.Length;
                UpdateSettingsTab();
            }
        }
    }
    
    public void PreviousTab(InputAction.CallbackContext context)
    {
        if (menu == Menu.settings)
        {
            if (context.started)
            {
                settingsTabIndex--;
                if (settingsTabIndex < 0) settingsTabIndex = settingsTab.Length - 1;
                UpdateSettingsTab();
            }
        }
    }

    private void UpdateSettingsTab()
    {
        for (int i = 0; i < settingsTab.Length; i++)
        {
            if (i == settingsTabIndex)
            {
                settingsTab[i].SetActive(true);
                settingsTabIcon[i].SetActive(true);
            }
            else
            {
                settingsTab[i].SetActive(false);
                settingsTabIcon[i].SetActive(false);
            }
        }
    }

    public void GoBack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(menu == Menu.settings) ChangeMenu(Menu.pause);
        }
    }
}


