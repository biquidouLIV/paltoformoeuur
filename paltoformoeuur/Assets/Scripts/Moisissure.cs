using UnityEngine;

public class Moisissure : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enterMoisi");
        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().Die();
        }
    }
}
