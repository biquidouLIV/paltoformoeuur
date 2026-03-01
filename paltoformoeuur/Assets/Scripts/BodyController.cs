using UnityEngine;
using UnityEngine.InputSystem;

public class BodyController : PlayerController
{
    [SerializeField] private float jumpHeight = 50;
    [SerializeField] private Vector2 jumpRaycastSize = new Vector2(1,1);
    [SerializeField] private Vector2 jumpRaycastOrigin = new Vector2(0,1);

    [SerializeField] private float coyoteTime = 0.5f;
    private float coyoteTimeCounter;
    
    
    [SerializeField] protected GameObject hand;
    [SerializeField] protected GameObject head;
    [SerializeField] protected HandController handController;
    [SerializeField] protected HeadController headController;
    
    
    private Vector2 rotationInput;
    private Vector2 rotation;
    
    private GameObject aim;
    public bool isAiming;


    private void Update()
    {
        if (CheckIfGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

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
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && coyoteTimeCounter > 0.0f)
        {
            elementRigidbody.AddForce(new Vector2(0,jumpHeight));
            coyoteTimeCounter = 0f;
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
        //return Physics2D.Raycast(transform.position, Vector2.down, jumpRaycastSize, ~LayerMask.GetMask("Player"));
        return Physics2D.BoxCast(transform.position + (Vector3)jumpRaycastOrigin, jumpRaycastSize, 0f, Vector2.down, 1, ~LayerMask.GetMask("Player"));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)jumpRaycastOrigin + Vector3.down, jumpRaycastSize);
    }

    
    
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started && PlayerManager.instance.controlledPart == PlayerManager.PlayerPart.body)
        {
            isAiming = true;
        }
        else if (context.canceled && isAiming && PlayerManager.instance.controlledPart == PlayerManager.PlayerPart.body)
        {
            isAiming = false;
            switch (PlayerManager.instance.selectedPart)
            {
                case PlayerManager.PlayerPart.hand:
                    SpawnHand();
                    break;
                case PlayerManager.PlayerPart.head:
                    SpawnHead();
                    break;
                case PlayerManager.PlayerPart.body:
                    break;
                default:
                    Debug.LogError("No selected part");
                    break;
            }
        }
    }
    
    private void SpawnHand()
    {
        handController.elementRigidbody.simulated = true; 
        elementRigidbody.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;

        hand.GetComponent<Rigidbody2D>().AddForce(new (rotation.x * 500, rotation.y * 500));
        rotation = Vector2.zero;
        
        PlayerManager.instance.EnableHand();
        hand.transform.SetParent(transform.parent);
    }
    
    
    private void SpawnHead()
    {
        headController.elementRigidbody.simulated = true;
        head.layer = 7;
        elementRigidbody.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;

        head.GetComponent<Rigidbody2D>().AddForce(new (rotation.x * 500, rotation.y * 500));
        rotation = Vector2.zero;
        
        PlayerManager.instance.EnableHead();
        head.transform.SetParent(transform.parent);
    }
}
