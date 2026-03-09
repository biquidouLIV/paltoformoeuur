using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;

    public void Play()
    {
        mainMenu.SetActive(false);
    }
    
    public void Quit()
    {
        mainMenu.SetActive(false);
    }
}
