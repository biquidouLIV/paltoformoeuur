using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
        // pas convaincu du return à la fin des fonctions
        return;
    }

    public void Quit()
    {
        Application.Quit();
        return;
    }
    
    
}
