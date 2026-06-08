using System.Collections;
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
            [SerializeField] private GameObject[] settingsTab1Objects;
            [SerializeField] private Slider[] slider;
            [SerializeField] private RectTransform settingsTabArrow;
            
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
        Cursor.visible = false;
        Time.timeScale = 1;
        eventSystem = EventSystem.current;
        currentSelectedButton = defaultMainMenuSelected;
        GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        ChangeMenu(Menu.mainMenu);
        transitionScreen.gameObject.SetActive(true);
        StartCoroutine(TransitionOpen());
    }
    private void Update()
    {
        if (currentSelectedButton != eventSystem.currentSelectedGameObject && menu == Menu.mainMenu)
        {
            MoveSelectionArrow();
        }

        if (currentSelectedButton != eventSystem.currentSelectedGameObject && menu == Menu.settings && settingsTabIndex == 0)
        {
            MoveSettings1SelectionArrow();
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
                settingsMenu.GetComponent<CanvasGroup>().interactable = false;
                mainPauseMenu.GetComponent<CanvasGroup>().interactable = true;
                eventSystem.SetSelectedGameObject(defaultMainMenuSelected);
                currentSelectedButton = defaultMainMenuSelected;
                

                if (menu == Menu.settings)
                {
                    SoundManager.instance.PlaySound(SoundManager.instance.UIButtonHover);
                    eventSystem.SetSelectedGameObject(buttons[1].gameObject);
                }
                
                HideSettingsMenu();
                ShowPauseMenu();
                break;
            
            case(Menu.settings):
                settingsMenu.GetComponent<CanvasGroup>().interactable = true;
                mainPauseMenu.GetComponent<CanvasGroup>().interactable = false;
                eventSystem.SetSelectedGameObject(defaultSettingsSelected);
                settingsTabIndex = 0;
                
                HidePauseMenu();
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
                        .DOAnchorPosY(eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition.y, arrowSpeed)
                        .SetUpdate(true)
                        .SetEase(arrowEase);
                }));
        }
        private void HidePauseMenu()
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
            if (eventSystem.currentSelectedGameObject == null) return;
            selectionArrow.DOAnchorPosY(eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition.y, arrowSpeed)
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
            slider[0].value = SoundManager.instance.mainVolume;
            slider[1].value = SoundManager.instance.soundEffectVolume;
            slider[2].value = SoundManager.instance.musicVolume;
            if(PlayerManager.instance != null) settingsTab1Objects[3].GetComponent<Toggle>().isOn = PlayerManager.instance.slowMo;
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
    
            settingsSelectionTabIcon.DOAnchorPosX(-2000, 0.2f)
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

                settingsTabArrow.DOAnchorPosX(850, 0.2f)
                    .SetUpdate(true);
            }
    
            if (index == 1)
            {
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
                settingsTabArrow.DOAnchorPosX(2000, 0.2f)
                    .SetUpdate(true);
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
        
        private void MoveSettings1SelectionArrow()
        {
            if (eventSystem.currentSelectedGameObject == null) return;

            float target = 250;
            if (eventSystem.currentSelectedGameObject == settingsTab1Objects[0]) target = 250;
            if (eventSystem.currentSelectedGameObject == settingsTab1Objects[1]) target = 100;
            if (eventSystem.currentSelectedGameObject == settingsTab1Objects[2]) target = -50;
            if (eventSystem.currentSelectedGameObject == settingsTab1Objects[3]) target = -200;
            
            settingsTabArrow.DOAnchorPosY(target, arrowSpeed)
                .SetUpdate(true)
                .SetEase(arrowEase);
            SoundManager.instance.PlaySound(SoundManager.instance.UIButtonHover);
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
        
        public void ChangeMainVolume()
        {
            SoundManager.instance.ChangeMainVolume(slider[0].value);
            SoundManager.instance.PlaySound(SoundManager.instance.UIButtonHover);
        }

        public void ChangeEffectVolume()
        {
            SoundManager.instance.ChangeEffectVolume(slider[1].value);
            SoundManager.instance.PlaySound(SoundManager.instance.UIButtonHover);
        }

        public void ChangeMusicVolume()
        {
            SoundManager.instance.ChangeMusicVolume(slider[2].value);
            SoundManager.instance.PlaySound(SoundManager.instance.UIButtonHover);
        }
        
        public void changeSlowMo()
        {
            PlayerManager.instance.slowMo = settingsTab1Objects[3].GetComponent<Toggle>().isOn;
        }
    #endregion
    
    public void ButtonClickSound()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.UIButtonClick);
    }
}
