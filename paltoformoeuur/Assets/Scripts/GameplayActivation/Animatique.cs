using UnityEngine;
using UnityEngine.SceneManagement;

public class Animatique : MonoBehaviour
{
    private void Start()
    {
        CameraManager.instance.ChangeTargetAnimatique();
    }

    public void EndAnimatique()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCount)
        {
            UIManager.instance.LoadScene(0);
        }
        UIManager.instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
