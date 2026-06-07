using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private float velocityToBreak;
    [SerializeField] private GameObject vfx;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            Rigidbody2D rigidbodyD = other.gameObject.GetComponent<Rigidbody2D>();
            if (rigidbodyD.linearVelocity.magnitude > velocityToBreak)
            {
                Destroy(gameObject);
                Instantiate(vfx,transform.position, Quaternion.identity);
                rigidbodyD.linearVelocity = new(rigidbodyD.linearVelocity.x / 2, rigidbodyD.linearVelocity.y / 2);
            }
        }
    }
}
