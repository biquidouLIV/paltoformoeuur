using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

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
        [SerializeField] private GameObject[] settingsTabIcon;
        [SerializeField] private RectTransform settingsSelectionTabIcon;
        [SerializeField] private float settingsTabSelectionSpeed = 0.2f;
        [SerializeField] private Ease settingsTabSelectionEase;
        private int settingsTabIndex = 0;
    
    [Header("transition")]
        [SerializeField] private RectTransform transitionScreen;
    
    
    
    private float actualTimeScale;
    private Menu menu;
    private EventSystem eventSystem;
    private GameObject currentSelectedButton;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        eventSystem = EventSystem.current;
        currentSelectedButton = defaultPauseSelected;
        PlayerManager.instance.PlayerInput.SwitchCurrentActionMap("Player");
        ChangeMenu(Menu.noMenu);
        
        
        
        transitionScreen.gameObject.SetActive(true);
        StartCoroutine(TransitionOpen());
    }

    private void Update()
    {
        if(menu != Menu.pause) return;
        if (currentSelectedButton != eventSystem.currentSelectedGameObject)
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
                Time.timeScale = 1;
                PlayerManager.instance.PlayerInput.SwitchCurrentActionMap("Player");
                
                
                //setup anims
                
                
                
                //anims
                pauseMenu.GetComponent<Image>().DOFade(0, 0.2f)
                    .SetUpdate(true)
                    .OnComplete((() =>
                    {
                        pauseMenu.SetActive(false);
                    }));
                header.DOAnchorPos(new Vector2(0, 200), 0.2f)
                    .SetUpdate(true)
                    .OnComplete((() =>
                    {
                        
                    }));
                
                break;
            
            case(Menu.pause):
                Time.timeScale = 0;
                eventSystem.SetSelectedGameObject(defaultPauseSelected);
                PlayerManager.instance.PlayerInput.SwitchCurrentActionMap("UI");
                settingsTabIndex = 0;
                
                pauseMenu.SetActive(true);
                defaultPauseMenu.SetActive(true);
                settingsMenu.SetActive(false);
                
                //setup anims
                
                //anims
                pauseMenu.GetComponent<Image>().DOFade(0.9f, 0.2f)
                    .SetUpdate(true);
                
                header.DOAnchorPos(new Vector2(0, (-150)), 0.2f)
                    .SetUpdate(true)
                    .OnComplete((() =>
                    {
                        
                    }));
                
                
                
                break;
            
            case(Menu.settings):
                eventSystem.SetSelectedGameObject(defaultSettingsSelected);
                pauseMenu.SetActive(true);
                defaultPauseMenu.SetActive(false);
                settingsMenu.SetActive(true);
                UpdateSettingsTab();
                break;
        }
        menu = newMenu;
    }

    private void MoveSelectionArrow()
    {
        selectionArrow.DOAnchorPosY(eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition.y, arrowSpeed)
            .SetUpdate(true)
            .SetEase(arrowEase);
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
                settingsSelectionTabIcon.DOAnchorPosX(settingsTabIcon[i].GetComponent<RectTransform>().anchoredPosition.x, arrowSpeed)
                            .SetUpdate(true)
                            .SetEase(settingsTabSelectionEase);
            }
            else
            {
                settingsTab[i].SetActive(false);
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


