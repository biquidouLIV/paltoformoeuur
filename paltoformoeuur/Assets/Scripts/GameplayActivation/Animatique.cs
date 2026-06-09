using UnityEngine;
using UnityEngine.SceneManagement;

public class Animatique : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(TransitionManager.instance.TransitionOpen());
    }

    public void EndAnimatique()
    {
        TransitionManager.instance.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
