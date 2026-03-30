using UnityEngine;

public class Crochet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            other.gameObject.GetComponent<HandController>().Accroche(this);
        }
    }
}
