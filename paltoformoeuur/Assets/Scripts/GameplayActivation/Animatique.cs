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
        UIManager.instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
