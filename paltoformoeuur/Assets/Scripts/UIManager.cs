using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    private float actualTimeScale;

    public void Play()
    {
        mainMenu.SetActive(false);
    }
    
    public void Quit()
    {
        mainMenu.SetActive(false);
    }

    public void Pause()
    {
        
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (pauseMenu.activeSelf)
        {
            actualTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else Time.timeScale = actualTimeScale;
    }
}
