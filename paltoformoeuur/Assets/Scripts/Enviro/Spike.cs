using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.Die();
        }
        if (other.gameObject.CompareTag("Body"))
        {
            SoundManager.instance.PlaySound(SoundManager.instance.deathFinal);
        }
    }
}