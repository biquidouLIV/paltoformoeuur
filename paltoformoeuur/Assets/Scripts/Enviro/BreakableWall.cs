using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private float velocityToBreak;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            Rigidbody2D rigidbodyD = other.gameObject.GetComponent<Rigidbody2D>();
            if (rigidbodyD.linearVelocity.magnitude > velocityToBreak)
            {
                Destroy(gameObject);
                rigidbodyD.linearVelocity = new(rigidbodyD.linearVelocity.x / 2, rigidbodyD.linearVelocity.y / 2);
            }
        }
    }
}
