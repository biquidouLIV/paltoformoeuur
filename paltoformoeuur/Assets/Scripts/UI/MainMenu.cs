using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private enum Menu
    {
        mainMenu,
        settings,
    }
    
    [Header("menus")]
        [SerializeField] private GameObject mainPauseMenu;
        [SerializeField] private GameObject settingsMenu;

    [Header("defaultSelectedButtons")]
        [SerializeField] private GameObject defaultMainMenuSelected;
        [SerializeField] private GameObject defaultSettingsSelected;

    [Header("pause menu")] 
        [SerializeField] private RectTransform selectionArrow;
        [SerializeField] private float arrowSpeed = 0.2f;
        [SerializeField] private Ease arrowEase;

        [SerializeField] private RectTransform header;
        [SerializeField] private RectTransform[] buttons;
        
    [Header("settings menu")]
        [SerializeField] private GameObject[] settingsTab;
        [SerializeField] private RectTransform[] settingsTabIcon;
        [SerializeField] private RectTransform settingsSelectionTabIcon;
        [SerializeField] private float settingsTabSelectionSpeed = 0.2f;
        [SerializeField] private Ease settingsTabSelectionEase;
        
        [Header("settings tab 1")]
            [SerializeField] private RectTransform[] settingsTab1Components;
        [Header("settings tab 2")] 
            [SerializeField] private RectTransform controller;
        
    [Header("transition")]
        [SerializeField] private RectTransform transitionScreen;
    
    
    
    private float actualTimeScale;
    private Menu menu;
    private EventSystem eventSystem;
    private GameObject currentSelectedButton;
    private int settingsTabIndex = 0;
    
    private void Start()
    {
        eventSystem = EventSystem.current;
        currentSelectedButton = defaultMainMenuSelected;
        GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        ChangeMenu(Menu.mainMenu);
        transitionScreen.gameObject.SetActive(true);
        StartCoroutine(TransitionOpen());
    }
    private void Update()
    {
        if(menu != Menu.mainMenu) return;
        if (currentSelectedButton != eventSystem.currentSelectedGameObject && menu == Menu.mainMenu)
        {
            MoveSelectionArrow();
        }

        currentSelectedButton = eventSystem.currentSelectedGameObject;

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
                SceneManager.LoadScene(scene);
            }));
    }
    private void ChangeMenu(Menu newMenu)
    {
        switch (newMenu)
        {
            case(Menu.mainMenu):
                eventSystem.SetSelectedGameObject(defaultMainMenuSelected);
                currentSelectedButton = eventSystem.currentSelectedGameObject;
                

                if (menu == Menu.settings)
                {
                    SoundManager.instance.PlaySound(SoundManager.instance.UIButtonHover);
                    eventSystem.SetSelectedGameObject(buttons[1].gameObject);
                }
                
                HideSettingsMenu();
                ShowPauseMenu();
                break;
            
            case(Menu.settings):
                eventSystem.SetSelectedGameObject(defaultSettingsSelected);
                settingsTabIndex = 0;
                
                HidePauseMenu(newMenu);
                ShowSettingsMenu();
                UpdateSettingsTab();
                break;
        }
        menu = newMenu;
    }

    #region PauseMenu
        private void ShowPauseMenu()
        {
            selectionArrow.anchoredPosition = buttons[0].anchoredPosition;
                    
            mainPauseMenu.GetComponent<Image>().DOFade(0.9f, 0.2f)
                .SetUpdate(true)
                .OnComplete((() =>
                {
                    defaultMainMenuSelected.GetComponent<CanvasGroup>().interactable = true;
                }));
                    
            header.DOAnchorPos(new Vector2(0, (-150)), 0.2f)
                .SetUpdate(true)
                .OnComplete((() =>
                {
                            
                }));
                    
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].DOAnchorPosY( 250 - 250 * i,0.2f + 0.1f * i)
                    .SetUpdate(true);
            }

            selectionArrow.DOSizeDelta(new Vector2(1100, 200f), 0.3f)
                .SetUpdate(true)
                .OnComplete((() =>
                {
                    selectionArrow
                        .DOAnchorPosY(
                            eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition.y,
                            arrowSpeed)
                        .SetUpdate(true)
                        .SetEase(arrowEase);
                }));
        }
        private void HidePauseMenu(Menu newMenu)
        {
            header.DOAnchorPos(new Vector2(0, 200), 0.2f)
                .SetUpdate(true);
                        
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].DOAnchorPosY(-1000,0.2f + 0.1f * i)
                        .SetUpdate(true);
                }
        
                selectionArrow.DOSizeDelta(new Vector2(5000, 200f), 0.3f)
                    .SetUpdate(true);
            }
        private void MoveSelectionArrow()
            {
                selectionArrow
                    .DOAnchorPosY(eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition.y, arrowSpeed)
                    .SetUpdate(true)
                    .SetEase(arrowEase);
                SoundManager.instance.PlaySound(SoundManager.instance.UIButtonHover);
            }
        
    #endregion

    #region SettingsMenu

        private void ShowSettingsMenu()
        {
            for (int i = 0; i < settingsTabIcon.Length; i++)
            {
                settingsTabIcon[i].DOAnchorPosY(50, 0.2f + i * 0.1f)
                    .SetUpdate(true)
                    .OnComplete((() =>
                    {
                        settingsMenu.GetComponent<CanvasGroup>().interactable = true;
                    }));
            }
            ShowSettingsTab(0);
        }
        private void HideSettingsMenu()
        {
            for (int i = 0; i < settingsTabIcon.Length; i++)
            {
                settingsTabIcon[i].DOAnchorPosY(200, 0.2f + i * 0.1f)
                    .SetUpdate(true)
                    .OnComplete((() =>
                    {
                        settingsMenu.GetComponent<CanvasGroup>().interactable = false;
                    }));
            }
    
            settingsSelectionTabIcon.DOAnchorPosX(-1100, 0.2f)
                .SetUpdate(true);
    
            HideSettingsTab(0);
            HideSettingsTab(1);
    
        }
        private void ShowSettingsTab(int index)
        {
            if (index == 0)
            {
                for (int i = 0; i < settingsTab1Components.Length; i++)
                {
                    settingsTab1Components[i].DOAnchorPosX(0, 0.2f + 0.1f * i)
                        .SetUpdate(true);
                }
            }
    
            if (index == 1)
            {
                Debug.Log("show tab" + index);
                controller.GetComponent<Image>().DOFade(1, 0.2f)
                    .SetUpdate(true);
            }
        }
        private void HideSettingsTab(int index)
        {
            if (index == 0)
            {
                for (int i = 0; i < settingsTab1Components.Length; i++)
                {
                    settingsTab1Components[i].DOAnchorPosX(-1920, 0.2f + 0.1f * i)
                        .SetUpdate(true);
                }
            }
            if (index == 1)
            {
                controller.GetComponent<Image>().DOFade(0, 0.2f)
                    .SetUpdate(true);
            }
        }
        private void UpdateSettingsTab()
        {
            for (int i = 0; i < settingsTab.Length; i++)
            {
                if (i == settingsTabIndex)
                {
                    SoundManager.instance.PlaySound(SoundManager.instance.UIButtonHover);
                    settingsSelectionTabIcon.DOAnchorPosX(settingsTabIcon[i].anchoredPosition.x, settingsTabSelectionSpeed)
                                .SetUpdate(true)
                                .SetEase(settingsTabSelectionEase);
                    ShowSettingsTab(i);
                }
                else
                {
                    HideSettingsTab(i);
                }
            }
        }
        
    #endregion

    #region Input /Buttons
        public void Settings()
        {
            ChangeMenu(Menu.settings);
            UpdateSettingsTab();
        }
        public void Quit()
        {
            Application.Quit();
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
        public void GoBack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if(menu == Menu.settings) ChangeMenu(Menu.mainMenu);
            }
        }
    #endregion
    
    public void ButtonClickSound()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.UIButtonClick);
    }
}
