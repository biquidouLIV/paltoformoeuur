using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private float strength;
    [SerializeField] private float baseStrength = 400;
    [SerializeField] private float delayBigJump;
    private float delayBigJumpCounter;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<BodyController>().bufferingTimeCounter > 0f || delayBigJumpCounter == 0)
        {
            other.rigidbody.AddForce(Vector3.up * strength);
        }
        else
        {
            other.rigidbody.AddForce(Vector3.up * baseStrength);
        }

        delayBigJumpCounter = delayBigJump;
        other.gameObject.GetComponent<BodyController>().hitBumper = 1f;
    }
}
