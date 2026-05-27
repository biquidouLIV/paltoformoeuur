using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
        return;
    }

    public void Quit()
    {
        Application.Quit();
        return;
    }
    
    
}
