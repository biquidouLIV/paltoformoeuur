using UnityEngine;

public class Moisissure : MonoBehaviour
{
    [SerializeField] private float ralentissement;
    [SerializeField] private float ralentissementInstantane;
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            Rigidbody2D rigidbody = other.GetComponent<Rigidbody2D>();
            rigidbody.angularDamping += ralentissement;
            rigidbody.linearVelocity /= ralentissementInstantane;
            return;
        }
        if (other.gameObject.CompareTag("Body"))
        {
            SoundManager.instance.PlaySound(SoundManager.instance.deathMold);
        }

        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.Die();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            other.GetComponent<Rigidbody2D>().angularDamping -= ralentissement;
        }
    }
}
