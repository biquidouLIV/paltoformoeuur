using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerAnimatique : MonoBehaviour
{
    public Animator animatiqueAnimator;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Body"))
        {
            UIManager.instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
