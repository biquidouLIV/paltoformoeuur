using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private enum Menu
    {
        main,
        settings,
    }
    
    [Header("Menu")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsMenu;
    
    [Header("Menu Default Button")]
        [SerializeField] private GameObject defaultMainSelected;
        [SerializeField] private GameObject defaultSettingsSelected;
    
    [Header("settings menu")]
        [SerializeField] private GameObject[] settingsTab;
        [SerializeField] private GameObject[] settingsTabIcon;
        private int settingsTabIndex = 0;
        
        
    [SerializeField] private RectTransform transitionScreen;
    private EventSystem eventSystem;
    private Menu menu;


    private void Start()
    {
        Time.timeScale = 1;
        eventSystem = EventSystem.current;
        transitionScreen.gameObject.SetActive(true);
        StartCoroutine(TransitionOpen());
        ChangeMenu(Menu.main);
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
                transitionScreen.DOKill();
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
            case(Menu.main):
                eventSystem.SetSelectedGameObject(defaultMainSelected);
                mainMenu.SetActive(true);
                settingsMenu.SetActive(false);
                break;
            case(Menu.settings):
                settingsTabIndex = 0;
                eventSystem.SetSelectedGameObject(defaultSettingsSelected);
                mainMenu.SetActive(false);
                settingsMenu.SetActive(true);
                break;
        }

        menu = newMenu;
    }

    public void Settings()
    {
        ChangeMenu(Menu.settings);
        UpdateSettingsTab();
    }
    

    public void NextTab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (menu == Menu.settings)
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
            if(menu == Menu.settings) ChangeMenu(Menu.main);
        }
    }
}
