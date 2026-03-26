using System;
using UnityEngine;

public class Moisissure : MonoBehaviour
{
    [SerializeField] private float ralentissement;
    [SerializeField] private float ralentissementInstantane;
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enterMoisissure");
        if (other.gameObject.CompareTag("Head"))
        {
            other.GetComponent<Rigidbody2D>().angularDamping += ralentissement;
            other.GetComponent<Rigidbody2D>().linearVelocity /= ralentissementInstantane;
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
            other.GetComponent<Rigidbody2D>().angularDamping -= ralentissement;
        }
    }
}
