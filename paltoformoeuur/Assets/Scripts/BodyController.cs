using UnityEngine;
using UnityEngine.InputSystem;

public class BodyController : PlayerController
{
    
    [Header("paramètres")]
        [SerializeField] private float jumpHeight = 50;
        [SerializeField] private float launchForce = 100;
        [SerializeField] private float coyoteTime = 0.2f;
        private float coyoteTimeCounter;
        [SerializeField] private float bufferingTime = 0.2f;
        private float bufferingTimeCounter;
        
    [Header("GD pas touche")]
        [SerializeField] private Vector2 jumpRaycastSize = new Vector2(1,1);
        [SerializeField] private Vector2 jumpRaycastOrigin = new Vector2(0,1);
        
    [Header("Refs")]
        [SerializeField] protected GameObject hand;
        [SerializeField] protected GameObject head;
        [SerializeField] protected HandController handController;
        [SerializeField] protected HeadController headController;
        [SerializeField] private Trajectory trajectory;
        [SerializeField] public BoxCollider2D colliderWithHead;
        [SerializeField] public BoxCollider2D colliderWithoutHead;
    

    [SerializeField] private AudioSource jumpSound;
    
    private Vector2 rotationInput;
    private Vector2 rotation;
    private GameObject aim;
    public bool isAiming;

    private void Update()
    {
        CheckJump();
        DisplayTrajectory();
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
        if (context.performed)
        {
            bufferingTimeCounter = bufferingTime;
        }
        if (context.performed && coyoteTimeCounter > 0.0f && elementRigidbody.linearVelocityY <= 0)
        {
            return;
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

    private void DisplayTrajectory()
    {
        
        if (!isAiming)
        {
            trajectory.HideTrajectory();
        }
        else
        {
            switch (PlayerManager.instance.selectedPart)
            {
                case PlayerManager.PlayerPart.head:
                    trajectory.TrajectoryCalcul(head.transform.position, rotation * launchForce * Time.fixedDeltaTime);
                    break;
                case PlayerManager.PlayerPart.hand:
                    trajectory.TrajectoryCalcul(hand.transform.position, rotation * launchForce * Time.fixedDeltaTime);
                    break;
            }
        }
    }
    
    private void CheckJump()
    {
        if (CheckIfGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        bufferingTimeCounter -= Time.deltaTime;
        if (bufferingTimeCounter > 0f && coyoteTimeCounter > 0.0f && elementRigidbody.linearVelocityY <= 0)
        {
            jumpSound.Play();
            elementRigidbody.linearVelocityY = 0f;
            elementRigidbody.AddForce(new Vector2(0,jumpHeight));
            coyoteTimeCounter = 0f;
            bufferingTimeCounter = 0f;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)jumpRaycastOrigin + Vector3.down, jumpRaycastSize);
    }
    
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started && PlayerManager.instance.controlledPart == PlayerManager.PlayerPart.body && PlayerManager.instance.selectedPart != PlayerManager.PlayerPart.body)
        {
            isAiming = true;
            Time.timeScale = 0.25f;
        }
        else if (context.canceled && isAiming && PlayerManager.instance.controlledPart == PlayerManager.PlayerPart.body)
        {
            Time.timeScale = 1f;
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
        else if (context.canceled)
        {
            Time.timeScale = 1f;
        }
    }
    
    private void SpawnHand()
    {
        handController.elementRigidbody.simulated = true; 
        elementRigidbody.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;

        hand.GetComponent<Rigidbody2D>().AddForce(rotation * launchForce);
        rotation = Vector2.zero;
        
        PlayerManager.instance.EnableHand();
        hand.transform.SetParent(transform.parent);
    }
    
    
    private void SpawnHead()
    {
        colliderWithHead.enabled = false;
        colliderWithoutHead.enabled = true;
        headController.elementRigidbody.simulated = true;
        head.layer = 7;
        elementRigidbody.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;

        head.GetComponent<Rigidbody2D>().AddForce(rotation * launchForce);
        rotation = Vector2.zero;
        
        PlayerManager.instance.EnableHead();
        head.transform.SetParent(transform.parent);
    }
}
