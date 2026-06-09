using UnityEngine;

public class TriggerAnimatique : MonoBehaviour
{
    public Animator animatiqueAnimator;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Body"))
        {
            CameraManager.instance.ChangeTargetAnimatique();
            PlayerManager.instance.PlayerInput.enabled = false;
            animatiqueAnimator.Play("EndAnimation");
        }
    }
}
