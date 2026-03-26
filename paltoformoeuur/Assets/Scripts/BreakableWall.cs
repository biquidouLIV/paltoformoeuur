using System;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private float velocityToBreak;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            if (other.relativeVelocity.magnitude > velocityToBreak)
            {
                Debug.Log("cassage du mur omg " + other.relativeVelocity.magnitude);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("pas assez de velocity " + other.relativeVelocity.magnitude);
            }
        }
    }
}
