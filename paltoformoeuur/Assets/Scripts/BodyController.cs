using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class BodyController : PlayerController
{
    
        [SerializeField] private float tempsAccroche;
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
        [SerializeField] public Animator bodyAnimator;
        [SerializeField] private AudioSource jumpSound;

    private float jumpHeight;
    private float launchForce;
    private float coyoteTime;
    private float coyoteTimeCounter;
    private float bufferingTime;
    private float bufferingTimeCounter;
    
    private Vector2 rotationInput;
    private Vector2 rotation;
    private GameObject aim;
    public bool isAiming;
    private PlayerPart aimingPart;
    private bool accroche = false;
    private Crochet currentCrochet;

    public override void Init(PlayerData data)
    {
        if (data is BodyData bodyData)
        {
            jumpHeight = bodyData.jumpHeight;
            launchForce = bodyData.launchForce;
            bufferingTime = bodyData.bufferingTime;
            coyoteTime = bodyData.coyoteTime;
        }
    }
            
    private void Update()
    {
        if (elementRigidbody.linearVelocityY < 0)
        {
            bodyAnimator.SetBool("IsFalling",true);
            bodyAnimator.SetBool("IsJumping",false);
            
        }
        else if(elementRigidbody.linearVelocityY > 0)
        {
            bodyAnimator.SetBool("IsJumping",true);
        }
        else
        {
            bodyAnimator.SetBool("IsJumping",false);
            bodyAnimator.SetBool("IsFalling",false);
        }
        
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
        
        if (!isAiming)
        {
            trajectory.HideTrajectory();
            bodyAnimator.SetBool("IsAimingHead",false);
            bodyAnimator.SetBool("IsAimingHand",false);
        }
        else
        {
            switch (aimingPart)
            {
                case PlayerPart.head:
                    trajectory.TrajectoryCalcul(head.transform.position, rotation * launchForce * Time.fixedDeltaTime);
                    break;
                case PlayerPart.hand:
                    trajectory.TrajectoryCalcul(hand.transform.position, rotation * launchForce * Time.fixedDeltaTime);
                    break;
            }
        }
    }
    
    public override void OnMove(InputAction.CallbackContext context)
    {
        if (accroche)
        {
            return;
        }
        if (isAiming)
        {
            bodyAnimator.SetBool("IsWalking",false);
            rotationInput = context.ReadValue<Vector2>();
            if (rotationInput.x + rotationInput.y > 0.1 || rotationInput.x + rotationInput.y < -0.1)
            {
                rotation = rotationInput.normalized;
            }

            moveInput = Vector2.zero;
        }
        else
        {
            bodyAnimator.SetBool("IsWalking",true);
            base.OnMove(context);

            if (moveInput.x > 0)
            {
                bodyAnimator.SetBool("IsGoingLeft", false);
            }
            else if(moveInput.x < 0)
            {
                bodyAnimator.SetBool("IsGoingLeft", true);
            }
        }

        if (context.canceled)
        {
            bodyAnimator.SetBool("IsWalking",false);
        }
    }

    public override void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bodyScript.bodyAnimator.SetBool("IsSprinting",true);
            sprintSpeed = sprintSpeedMultiplier;
        }

        if (context.canceled)
        {
            bodyScript.bodyAnimator.SetBool("IsSprinting",false);
            sprintSpeed = 1;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (accroche && context.started)
        {
            accroche = false;
            currentCrochet = null;
            elementRigidbody.simulated = true;
        }
        if (accroche)
        {
            return;
        }
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
        return Physics2D.BoxCast(transform.position + (Vector3)jumpRaycastOrigin, jumpRaycastSize, 0f, Vector2.down, 1, ~LayerMask.GetMask("Player","Checkpoint"));
    }

    private void DisplayTrajectory()
    {
        
        if (!isAiming)
        {
            trajectory.HideTrajectory();
        }
        else
        {
            switch (aimingPart)
            {
                case PlayerPart.head:
                    trajectory.TrajectoryCalcul(head.transform.position, rotation * launchForce * Time.fixedDeltaTime);
                    break;
                case PlayerPart.hand:
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

    public void OnAimHead(InputAction.CallbackContext context)
    {
        if (context.started && !isAiming && PlayerManager.instance.headOnBody)
        {
            isAiming = true;
            Time.timeScale = 0.25f;
            bodyAnimator.SetBool("IsAimingHead",true);
            aimingPart = PlayerPart.head;
        }
        else if (context.canceled && isAiming && aimingPart == PlayerPart.head  && PlayerManager.instance.headOnBody)
        {
            Time.timeScale = 1f;
            isAiming = false;
            SpawnHead();
            aimingPart = default;
        }
        else if (context.canceled)
        {
            Time.timeScale = 1f;
        }
    }
    
    public void OnAimHand(InputAction.CallbackContext context)
    {
        if (context.started && !isAiming)
        {
            isAiming = true;
            Time.timeScale = 0.25f;
            bodyAnimator.SetBool("IsAimingHand",true);
            aimingPart = PlayerPart.hand;
        }
        else if (context.canceled && isAiming && aimingPart == PlayerPart.hand)
        {
            Time.timeScale = 1f;
            isAiming = false;
            SpawnHand();
            aimingPart = default;
        }
        else if (context.canceled)
        {
            Time.timeScale = 1f;
        }
    }
    
    private void SpawnHand()
    {
        bodyAnimator.SetBool("IsArmless", true);
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
        bodyAnimator.SetBool("IsHeadless", true);
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
    
    public override void Die()
    {
        bodyAnimator.SetTrigger("Die");
        transform.position = PlayerManager.instance.checkpointTransform;
    }
    
    public override void Accroche(Crochet crochet, FallingPlatform fallingPlatform)
    {
        accroche = true;
        currentCrochet = crochet;
        elementRigidbody.simulated = false;
        moveInput = Vector2.zero;
        transform.DOMove(crochet.gameObject.transform.position - new Vector3(0, 0.8f, 0), tempsAccroche)
            .OnComplete(() =>
            {
                gameObject.transform.parent = currentCrochet.transform;
                fallingPlatform.falling = true;
            });
    }
}
