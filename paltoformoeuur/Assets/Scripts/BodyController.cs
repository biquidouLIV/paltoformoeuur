using UnityEngine;
using UnityEngine.InputSystem;

public class BodyController : PlayerController
{
    [SerializeField] private float jumpHeight = 50;
    [SerializeField] private float jumpRaycastSize = 1;
    
    [SerializeField] protected GameObject hand;
    [SerializeField] protected HandController handController;
    
    private Vector2 rotationInput;
    private Vector2 rotation;
    
    private GameObject aim;
    private bool isAiming;
    
    public override void OnMove(InputAction.CallbackContext context)
    {
        if (isAiming)
        {
            rotationInput = context.ReadValue<Vector2>();
            if (rotationInput.x + rotationInput.y > 0.1 || rotationInput.x + rotationInput.y < -0.1)
            {
                rotation = rotationInput.normalized;
            }
            moveInput = Vector2.zero;
        }
        else
        {
            base.OnMove(context);
        }
    }
    
    public override void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && CheckIfGrounded())
        {
            StopAllCoroutines();
            elementRigidbody.AddForce(new Vector2(0,jumpHeight));
        }

        if (context.canceled)
        {
            if (elementRigidbody.linearVelocityY > 0)
            {
                elementRigidbody.linearVelocityY /= 2;
            }
        }
    }
    
    private bool CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, jumpRaycastSize, ~LayerMask.GetMask("Player"));
        return hit;
    }
    
    
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAiming = true;
        }
        else if (context.canceled && isAiming)
        {
            isAiming = false;
            SpawnHand();
        }
    }
    
    private void SpawnHand()
    {
        handController.elementRigidbody.simulated = true; 
        elementRigidbody.linearVelocity = Vector2.zero;
        elementRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        moveInput = Vector2.zero;

        hand.GetComponent<Rigidbody2D>().AddForce(new (rotation.x * 500, rotation.y * 500));
        rotation = Vector2.zero;
        
        PlayerManager.instance.EnableHand();
        hand.transform.SetParent(transform.parent);
    }
}
