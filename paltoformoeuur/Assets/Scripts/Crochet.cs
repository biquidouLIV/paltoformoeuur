using System.Collections;
using UnityEngine;

public class Crochet : MonoBehaviour
{
    [SerializeField] private FallingPlatform fallingPlatform;
    [SerializeField] private float delayOnLeaving = 2;
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Hand") || other.gameObject.CompareTag("Body"))
        {
            StartCoroutine(Active());
        }
    }

    private IEnumerator Active()
    {
        yield return new WaitForSeconds(delayOnLeaving);
        isAvailable = true;
    }
}
