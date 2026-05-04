using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrochetBalance : Crochet
{
    [SerializeField] private float delayOnLeaving = 1;
    private bool isAvailable = true;
    private PlayerController playerController;

    public void Move(InputAction.CallbackContext context)
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAvailable)
        {
            return;
        }
        if (other.gameObject.CompareTag("Hand") || other.gameObject.CompareTag("Body"))
        {
            playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.Accroche(this);
        }

        isAvailable = false;
    }
    
    public override IEnumerator Active()
    {
        yield return new WaitForSeconds(delayOnLeaving);
        isAvailable = true;
    }
}
