using UnityEngine;
using UnityEngine.Serialization;

public class Moisissure : MonoBehaviour
{
    [FormerlySerializedAs("ralentissement")] [SerializeField] private float slowdown;
    [FormerlySerializedAs("ralentissementInstantane")] [SerializeField] private float instantSlowdown;
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            other.GetComponent<Rigidbody2D>().angularDamping += slowdown;
            other.GetComponent<Rigidbody2D>().linearVelocity /= instantSlowdown;
            return;
        }
        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().Die();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            other.GetComponent<Rigidbody2D>().angularDamping -= slowdown;
        }
    }
}
