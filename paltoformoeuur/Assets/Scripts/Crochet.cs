using System.Collections;
using UnityEngine;

public class Crochet : MonoBehaviour
{
    [SerializeField] private FallingPlatform fallingPlatform;
    [SerializeField] private float delayOnLeaving = 1;
    private bool isAvailable = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAvailable)
        {
            return;
        }
        if (other.gameObject.CompareTag("Hand") || other.gameObject.CompareTag("Body"))
        {
            other.gameObject.GetComponent<PlayerController>().Accroche(this, fallingPlatform);
        }

        isAvailable = false;
    }
    
    public IEnumerator Active()
    {
        fallingPlatform.falling = false;
        yield return new WaitForSeconds(delayOnLeaving);
        isAvailable = true;
    }
}
