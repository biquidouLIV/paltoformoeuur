using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI;

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

    [Header("defaultSelectedButtons")]
        [SerializeField] private GameObject defaultPauseSelected;
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
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.gameObject.SetActive(true);
        eventSystem = EventSystem.current;
        currentSelectedButton = defaultPauseSelected;
        PlayerManager.instance.PlayerInput.SwitchCurrentActionMap("Player");
        ChangeMenu(Menu.noMenu);
        transitionScreen.gameObject.SetActive(true);
        StartCoroutine(TransitionOpen());
    }
    private void Update()
    {
        if (currentSelectedButton != eventSystem.currentSelectedGameObject && menu == Menu.pause)
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
            case(Menu.noMenu):
                Time.timeScale = 1;
                defaultPauseMenu.GetComponent<CanvasGroup>().interactable = false;
                settingsMenu.GetComponent<CanvasGroup>().interactable = false;
                HidePauseMenu(newMenu);
                HideSettingsMenu();
                break;
            
            case(Menu.pause):
                Time.timeScale = 0;
                defaultPauseMenu.GetComponent<CanvasGroup>().interactable = true;
                settingsMenu.GetComponent<CanvasGroup>().interactable = false;
                eventSystem.SetSelectedGameObject(defaultPauseSelected);
                SoundManager.instance.PlaySound(SoundManager.instance.UIButtonHover);

                if (menu == Menu.noMenu)
                {
                    SoundManager.instance.PlaySound(SoundManager.instance.pause);
                }
                
                if (menu == Menu.settings)
                {
                    eventSystem.SetSelectedGameObject(buttons[1].gameObject);
                }
                
                currentSelectedButton = eventSystem.currentSelectedGameObject;
                HideSettingsMenu();
                ShowPauseMenu();
                break;
            
            case(Menu.settings):
                defaultPauseMenu.GetComponent<CanvasGroup>().interactable = false;
                settingsMenu.GetComponent<CanvasGroup>().interactable = true;
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

            pauseMenu.GetComponent<Image>().DOFade(0.9f, 0.2f)
                .SetUpdate(true);
                    
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
                        .SetEase(arrowEase)
                        .OnComplete((() =>
                        {
                            PlayerManager.instance.PlayerInput.SwitchCurrentActionMap("UI");
                        }));
                }));
        }
        private void HidePauseMenu(Menu newMenu)
            {
                if (newMenu == Menu.noMenu)
                {
                    pauseMenu.GetComponent<Image>().DOFade(0, 0.2f)
                        .SetUpdate(true)
                        .OnComplete((() =>
                        {
                                    
                        }));
                }
        
                        
                header.DOAnchorPos(new Vector2(0, 200), 0.2f)
                    .SetUpdate(true)
                    .OnComplete((() =>
                    {
                        if(newMenu == Menu.noMenu) PlayerManager.instance.PlayerInput.SwitchCurrentActionMap("Player");
                    }));
                        
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
                    .SetUpdate(true);
            }
            slider[0].value = SoundManager.instance.mainVolume;
            slider[1].value = SoundManager.instance.soundEffectVolume;
            slider[2].value = SoundManager.instance.musicVolume;
            settingsTab1Objects[3].GetComponent<Toggle>().isOn = PlayerManager.instance.slowMo;
            ShowSettingsTab(0);
        }
        private void HideSettingsMenu()
        {
            for (int i = 0; i < settingsTabIcon.Length; i++)
            {
                settingsTabIcon[i].DOAnchorPosY(200, 0.2f + i * 0.1f)
                    .SetUpdate(true);
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
                if(menu == Menu.settings) ChangeMenu(Menu.pause);
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


