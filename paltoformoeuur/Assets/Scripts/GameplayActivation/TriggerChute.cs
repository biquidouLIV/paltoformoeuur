using System.Collections;
using UnityEngine;

public class TriggerChute : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Body") && PlayerManager.instance.headOnBody)
        {
            PlayerManager.instance.bodyController.bodyAnimator.Play("Chute");
            PlayerManager.instance.PlayerInput.enabled = false;
            gameObject.SetActive(false);
        }
    }
}