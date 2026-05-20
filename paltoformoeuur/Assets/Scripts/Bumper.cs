using System;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private float strength = 16;
    [SerializeField] private float baseStrength = 8;
    [SerializeField] private float delayBigJump = 2f;
    private float delayBigJumpCounter;

    private void Update()
    {
        delayBigJumpCounter -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.rigidbody.linearVelocityX = 0;
        switch (other.gameObject.tag)
        {
            case "Hand":
                other.rigidbody.linearVelocityY = strength;
                break;
            case "Head":
                other.rigidbody.linearVelocityY = strength;
                break;
            case "Body":
                if (other.gameObject.GetComponent<BodyController>().bufferingTimeCounter > 0f || delayBigJumpCounter <= 0)
                {
                    other.rigidbody.linearVelocityY = strength;
                }
                else
                {
                    other.rigidbody.linearVelocityY = baseStrength;
                }

                other.gameObject.GetComponent<BodyController>().hitBumper = true;
                delayBigJumpCounter = delayBigJump;
                break;
        }
    }
}
