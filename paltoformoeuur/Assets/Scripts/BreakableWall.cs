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
                Destroy(gameObject);
            }
        }
    }
}
