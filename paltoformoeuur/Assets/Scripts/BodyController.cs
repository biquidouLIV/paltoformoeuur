using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class BodyController : PlayerController
{
    
        [SerializeField] private float tempsAccroche;
    [Header("GD pas touche")]
        [SerializeField] private Vector2 jumpRaycastSize = new (1,1);
        [SerializeField] private Vector2 jumpRaycastOrigin = new (0,1);
        
    [Header("Refs")]
        [SerializeField] protected GameObject playerParent;
        [SerializeField] protected GameObject hand;
        [SerializeField] protected GameObject head;
        [SerializeField] protected HandController handController;
        [SerializeField] protected HeadController headController;
        [SerializeField] private Trajectory trajectory;
        [SerializeField] public BoxCollider2D colliderWithHead;
        [SerializeField] public BoxCollider2D colliderWithoutHead;
        [SerializeField] public Animator bodyAnimator;
        [SerializeField] private AudioSource jumpSound;

        [Header("Temp")] public float hitBumper;
        [SerializeField] private float distanceVisionTete;

    private float jumpHeight;
    private float launchForce;
    private float coyoteTime;
    private float coyoteTimeCounter;
    private float bufferingTime;
    public float bufferingTimeCounter;
    
    private Vector2 rotationInput;
    private Vector2 rotation;
    private GameObject aim;
    public bool isAiming;
    private PlayerPart aimingPart;
    private bool accroche;
    private CrochetPlatform currentCrochet;
    public bool isGrounded;
    private float timeSinceLastJump;
    private float jumpMinimumDelay = 0.3f;

    public float distanceWithGround;
    public bool canThrowHead;
    public bool canThrowHand;
    public override void Init(PlayerData data)
    {
        if (data is BodyData bodyData)
        {
            jumpHeight = bodyData.jumpHeight;
            launchForce = bodyData.launchForce;
            bufferingTime = bodyData.bufferingTime;
            coyoteTime = bodyData.coyoteTime;
            head.SetActive(false);
            hand.SetActive(false);
            timeSinceLastJump = jumpMinimumDelay;
        }
    }
            
    private void Update()
    {
        AnimationGestion();
        UpdateVariableJump();
        CheckJump();
        GestionVise();
        CheckDistanceWithGround();
    }

    private void AnimationGestion()
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
    }

    private void UpdateVariableJump()
    {
        if (CheckIfGrounded())
        {
            isGrounded = true;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            isGrounded = false;
            coyoteTimeCounter -= Time.deltaTime;
        }

        hitBumper = Mathf.Max(hitBumper - Time.deltaTime, 0);
        bufferingTimeCounter -= Time.deltaTime;
        timeSinceLastJump += Time.deltaTime;
    }

    private void CheckJump()
    {
        if (bufferingTimeCounter > 0f && coyoteTimeCounter > 0.0f && timeSinceLastJump > jumpMinimumDelay && (hitBumper <= 0 || CheckIfGrounded()))
        {
            //jumpSound.Play();
            timeSinceLastJump = 0;
            elementRigidbody.linearVelocityY = 0;
            elementRigidbody.linearVelocityY = jumpHeight;
            coyoteTimeCounter = 0f;
            bufferingTimeCounter = 0f;
        }
    }
    
    private void GestionVise()
    {
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
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
            else if(moveInput.x < 0)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
        }

        if (context.canceled)
        {
            bodyAnimator.SetBool("IsWalking",false);
        }
    }

    public override void OnSprint(InputAction.CallbackContext context)
    {
        if(sprintSpeedMultiplier == 1) return;
        
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
            Decroche();
        }
        if (accroche)
        {
            return;
        }
        if (context.performed)
        {
            bufferingTimeCounter = bufferingTime;
        }
        
        if (context.canceled && hitBumper <= 0)
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
        return Physics2D.BoxCast(transform.position + (Vector3)jumpRaycastOrigin, jumpRaycastSize, 0f, Vector2.down, 1, ~LayerMask.GetMask("Player","Checkpoint","Bumper"));
    }

    private void CheckDistanceWithGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, ~LayerMask.GetMask("Player", "Checkpoint","Bumper"));
        distanceWithGround = hit.distance;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)jumpRaycastOrigin + Vector3.down, jumpRaycastSize);
    }

    public void OnAimHead(InputAction.CallbackContext context)
    {
        
        if (context.started && !isAiming)
        {
            if(head.activeSelf) return;
            isAiming = true;
            Time.timeScale = 0.25f;
            bodyAnimator.SetBool("IsAimingHead",true);
            aimingPart = PlayerPart.head;
        }
        
        else if (context.canceled && isAiming && aimingPart == PlayerPart.head && PlayerManager.instance.headOnBody)
        {     
            SpawnHead();
            
            Time.timeScale = 1f;
            isAiming = false;
            if(canThrowHead)return;
            if (head.activeSelf) return;
            canThrowHead = true;
            aimingPart = default;
        }
        else if (context.canceled)
        {
            Time.timeScale = 1f;
        }
    }
    
    public void OnAimHand(InputAction.CallbackContext context)
    {
        if (context.started && !isAiming && PlayerManager.instance.handOnBody)
        {
            if(hand.activeSelf) return;
            isAiming = true;
            Time.timeScale = 0.25f;
            bodyAnimator.SetBool("IsAimingHand",true);
            aimingPart = PlayerPart.hand;
        }
        else if (context.canceled && isAiming && aimingPart == PlayerPart.hand && PlayerManager.instance.handOnBody)
        {
            SpawnHand();
            Time.timeScale = 1f;
            isAiming = false;
            if(canThrowHand)return;
            if(hand.activeSelf)return;
            canThrowHand = true;
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
        bodyAnimator.SetBool("IsWalking",false);
        bodyAnimator.SetBool("IsSprinting",false);
        
        hand.SetActive(true);
        handController.elementRigidbody.simulated = true; 
        elementRigidbody.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;

        handController.elementRigidbody.AddForce(rotation * launchForce);
        rotation = Vector2.zero;
        
        PlayerManager.instance.EnableHand();
        hand.transform.SetParent(transform.parent);
    }
    
    
    private void SpawnHead()
    {
        bodyAnimator.SetBool("IsHeadless", true);
        head.SetActive(true);
        colliderWithHead.enabled = false;
        colliderWithoutHead.enabled = true;
        headController.elementRigidbody.simulated = true;
        head.layer = 7;
        elementRigidbody.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;
        
        headController.elementRigidbody.AddForce(rotation * launchForce);
        rotation = Vector2.zero;
        
        PlayerManager.instance.EnableHead();
        CameraManager.instance.ChangeTarget(PlayerPart.head);
        head.transform.SetParent(transform.parent);
    }
    
    public override void Die()
    {
        bodyAnimator.SetTrigger("Die");
    }

    
    //event dans animation de mort
    public void Respaw()
    {
        CameraManager.instance.CameraOnRespawn();
        transform.position = PlayerManager.instance.checkpointTransform;
        
        if (Vector3.Distance(transform.position, head.transform.position) > distanceVisionTete)
        {
            PlayerManager.instance.OnRecallHand();
            PlayerManager.instance.OnRecallHead();
        }
        else if (Vector3.Distance(head.transform.position, hand.transform.position) > distanceVisionTete)
        {
            PlayerManager.instance.OnRecallHand();
        }
    }
    
    public override void Accroche(CrochetPlatform crochet, FallingPlatform fallingPlatform)
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
    
    public override void Decroche()
    {
        gameObject.transform.parent = playerParent.transform;
        StartCoroutine(currentCrochet.Active());
        accroche = false;
        //c'est de la part de maxence ducoup je les ai pas retiré
        Debug.Log("kk");
        Debug.Log("kk");
        Debug.Log("kk");
        Debug.Log("kk");
        currentCrochet = null;
        elementRigidbody.simulated = true;
    }
}
