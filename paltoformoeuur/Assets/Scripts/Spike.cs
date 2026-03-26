using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enterSpike");
        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().Die();
        }
    }
}
