using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class BodyController : PlayerController
{
    [SerializeField] private Vector2 defaultRotationInput = new Vector2(0.8f, 0.6f);
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

        [Header("Temp")] public bool hitBumper;
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
    public bool accroche;
    private Crochet currentCrochet;
    private float timeSinceLastJump;
    private float jumpMinimumDelay = 0.3f;
    private float delayZoomHead;

    public float distanceWithGround;
    public bool canThrowHead;
    public bool canThrowHand;
    public bool isDying;
    private SpriteRenderer sprite;
    
    public override void Init(PlayerData data)
    {
        if (data is BodyData bodyData)
        {
            jumpHeight = bodyData.jumpHeight;
            launchForce = bodyData.launchForce;
            bufferingTime = bodyData.bufferingTime;
            coyoteTime = bodyData.coyoteTime;
            delayZoomHead = bodyData.delayZoomHead;
            timeSinceLastJump = jumpMinimumDelay;
            head.SetActive(false);
            hand.SetActive(false);
            isDying = false;
            sprite = GetComponent<SpriteRenderer>();
        }
    }
            
    protected void Update()
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
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        bufferingTimeCounter -= Time.deltaTime;
        timeSinceLastJump += Time.deltaTime;
    }

    private void CheckJump()
    {
        if ((bufferingTimeCounter > 0f && coyoteTimeCounter > 0.0f && timeSinceLastJump > jumpMinimumDelay && !hitBumper) || (bufferingTimeCounter > 0f && CheckIfGrounded()))
        {
            if(isDying)return;
            //jumpSound.Play();
            SoundManager.instance.PlaySound(SoundManager.instance.jump);
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
            if (rotation.magnitude <= 0.1)
            {
                rotation = defaultRotationInput;
                if (sprite.flipX)
                {
                    rotation.x = -defaultRotationInput.x;
                }
            }
            
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

            if (CheckIfGrounded())
            {
                moveInput = Vector2.zero;
            }
        }
        else
        {
            bodyAnimator.SetBool("IsWalking",true);
            base.OnMove(context);

            if (moveInput.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if(moveInput.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        if (context.canceled)
        {

            bodyAnimator.SetBool("IsWalking",false);
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
        
        if (context.canceled && !hitBumper)
        {
            if (elementRigidbody.linearVelocityY > 0)
            {
                elementRigidbody.linearVelocityY /= 2;
            }
        }
    }
    
    private bool CheckIfGrounded()
    {
        bool onFloor = Physics2D.BoxCast(transform.position + (Vector3)jumpRaycastOrigin, jumpRaycastSize, 0f,
            Vector2.down, 1, ~LayerMask.GetMask("Player", "Checkpoint", "Bumper", "Ignore Raycast"));
        if (onFloor)
        {
            hitBumper = false;
        }
        return onFloor;
    }

    private void CheckDistanceWithGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, ~LayerMask.GetMask("Player", "Checkpoint","Bumper"));
        distanceWithGround = hit.distance;
    }

    public IEnumerator Fall()
    {
        rotation = Vector2.right;
        SpawnHead();
        CameraManager.instance.HeadZoom();
        yield return new WaitForSeconds(delayZoomHead);
        CameraManager.instance.UnZoom();
    }

    public void FallSound()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.chute);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)jumpRaycastOrigin + Vector3.down, jumpRaycastSize);
    }

    public void OnAimHead(InputAction.CallbackContext context)
    {
        if(accroche) return;
        
        if (headController.isRecalling)
        {
            return;
        }
        
        if (context.started && !isAiming)
        {
            StartCoroutine(VelocityWhenSpawnHand());
            if(head.activeSelf) return;
            isAiming = true;
            if (PlayerManager.instance.slowMo) Time.timeScale = PlayerManager.instance.slowMoValue;
            bodyAnimator.SetBool("IsAimingHead",true);
            SoundManager.instance.PlaySound(SoundManager.instance.aim);
            aimingPart = PlayerPart.head;
        }
        
        else if (context.canceled && isAiming && aimingPart == PlayerPart.head && PlayerManager.instance.headOnBody)
        {     
            SpawnHead();
            SoundManager.instance.PlaySound(SoundManager.instance.launch);
            if (PlayerManager.instance.slowMo) Time.timeScale = 1f;
            isAiming = false;
            if(canThrowHead)return;
            if (head.activeSelf) return;
            canThrowHead = true;
            aimingPart = default;
        }
        else if (context.canceled && PlayerManager.instance.slowMo)
        {
            Time.timeScale = 1f;
        }
    }
    
    public void OnAimHand(InputAction.CallbackContext context)
    {
        if(accroche) return;
        
        if (context.started && !isAiming && PlayerManager.instance.handOnBody)
        {
            StartCoroutine(VelocityWhenSpawnHand());
            if(hand.activeSelf) return;
            isAiming = true;
            if (PlayerManager.instance.slowMo) Time.timeScale = PlayerManager.instance.slowMoValue;
            bodyAnimator.SetBool("IsAimingHand",true);
            SoundManager.instance.PlaySound(SoundManager.instance.aim);
            aimingPart = PlayerPart.hand;
        }
        else if (context.canceled && isAiming && aimingPart == PlayerPart.hand && PlayerManager.instance.handOnBody)
        {
            SpawnHand();
            SoundManager.instance.PlaySound(SoundManager.instance.launch);
            if (PlayerManager.instance.slowMo) Time.timeScale = 1f;
            isAiming = false;
            if (canThrowHand) return;
            if (hand.activeSelf) return;
            canThrowHand = true;
            aimingPart = default;
        }
        else if (context.canceled && PlayerManager.instance.slowMo)
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
        StartCoroutine(VelocityWhenSpawnHand());

        handController.elementRigidbody.AddForce(rotation * launchForce);
        rotation = Vector2.zero;
        
        PlayerManager.instance.EnableHand();
        hand.transform.SetParent(null);
    }

    private IEnumerator VelocityWhenSpawnHand()
    {
        if (CheckIfGrounded())
        {
            moveInput = Vector2.zero;
        }
        else
        {
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(VelocityWhenSpawnHand());
        }
    }
    
    private void SpawnHead()
    {
        bodyAnimator.SetBool("IsHeadless", true);
        head.SetActive(true);
        colliderWithHead.enabled = false;
        colliderWithoutHead.enabled = true;
        headController.elementRigidbody.simulated = true;
        head.layer = 7;
        
        headController.elementRigidbody.AddForce(rotation * launchForce);
        rotation = Vector2.zero;
        
        PlayerManager.instance.EnableHead();
        CameraManager.instance.ChangeTarget(PlayerPart.head);
        Parallaxe.ChangeTarget(PlayerPart.head);
        head.transform.SetParent(null);
    }
    
    public override void Die()
    {
        bodyAnimator.SetTrigger("Die");
        isDying = true;
        PlayerManager.instance.PlayerInput.enabled = false;
        elementRigidbody.linearVelocityX = 0;
    }

    
    //event dans animation de mort
    public void Respawn()
    {
        StartCoroutine(CameraManager.instance.CameraOnRespawn());
        transform.position = PlayerManager.instance.checkpointTransform;
        SoundManager.instance.PlaySound(SoundManager.instance.respawnCheckpoint);
        
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

    //event dans l'anim de respawn
    public void ActiveInput()
    {
        isDying = false;
        PlayerManager.instance.PlayerInput.enabled = true;
    }
    
    public override void Accroche(CrochetBalance crochet)
    {
        isAiming = false;
        bodyAnimator.SetBool("IsWalking", false);
        bodyAnimator.SetBool("IsAiming", false);
        bodyAnimator.SetBool("IsFalling", false);
        bodyAnimator.SetBool("IsBalancing", true);
        accroche = true;
        bool fromLeft = crochet.transform.position.x < transform.position.x;
        currentCrochet = crochet;
        elementRigidbody.simulated = false;
        moveInput = Vector2.zero;
        transform.DOMove(crochet.gameObject.transform.position - new Vector3(0, 2f, 0), tempsAccroche)
            .OnComplete(() =>
            {
                gameObject.transform.parent = currentCrochet.transform;
                crochet.StartRotation(fromLeft);
            });
    }
    
    public override void Decroche()
    {
        bodyAnimator.SetBool("IsBalancing", false);
        gameObject.transform.parent = playerParent.transform;
        gameObject.transform.eulerAngles = Vector3.zero;
        elementRigidbody.simulated = true;
        StartCoroutine(currentCrochet.OnLeave(elementRigidbody));
        accroche = false;
        currentCrochet = null;
    }

    public void Land()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.land);
    }

    public void PlayStepSound()
    {
        SoundManager.instance.PlayStepSound();
    }
    
}