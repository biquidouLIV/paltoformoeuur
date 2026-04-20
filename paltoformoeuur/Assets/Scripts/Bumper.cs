using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private float strength;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        other.rigidbody.AddForce(Vector3.up * strength);
        other.gameObject.GetComponent<BodyController>().hitBumper = 1f;
    }
}
