using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : PlayerController
{
        [SerializeField] private float tempsAccroche;


    [Header("Refs")]
        [SerializeField] public Animator handAnimator;


    private float dashSpeed;
    private float dashDuration;
    private float dashCooldown;
    private int recallSpeed;
    
    private bool canDash = true;
    private bool accroche;
    private Crochet currentCrochet;
    private int direction = 1;

    public override void Init(PlayerData data)
    {
        if (data is HandData handData)
        {
            dashSpeed = handData.dashSpeed;
            dashDuration = handData.dashDuration;
            dashCooldown = handData.dashCooldown;
            recallSpeed = handData.recallSpeed;
        }
    }

    protected void Update()
    {
        if (elementRigidbody.linearVelocityY < 0f)
        {
            handAnimator.SetBool("IsFalling",true);
        }
        else
        {
            handAnimator.SetBool("IsFalling",false);
        }
    }

    public override void OnMove(InputAction.CallbackContext context)
    {
        if (accroche)
        {
            return;
        }
        
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if(moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        
        handAnimator.SetBool("IsWalking", true);
        base.OnMove(context);
        
        if (moveInput.x > 0)
        {
            direction = 1;
            handAnimator.SetBool("IsGoingLeft", false);
        }
        else if(moveInput.x < 0)
        {
            direction = -1;
            handAnimator.SetBool("IsGoingLeft", true);
        }

        if (context.canceled)
        {
            handAnimator.SetBool("IsWalking",false);
        }
    }

    //ca s'appelle jump mais c'est un dash
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("ez");
        }
        if (context.performed && accroche)
        {
            Decroche();
        }
        else if (context.performed && canDash && !accroche)
        {
            StartCoroutine(Dash());
        }
    }

    public override void Recall()
    {
        if (currentCrochet != null)
        {
            Decroche();
        }
        PlayerManager.instance.PlayerInput.enabled = false;
        base.Recall();
        transform.DOLocalMove(PlayerManager.instance.handAnchorPosition, Vector2.Distance(transform.position, player.transform.position) / recallSpeed)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
                {
                    bodyScript.bodyAnimator.SetBool("IsArmless",false);
                    DisableElement();
                    PlayerManager.instance.handOnBody = true;
                    PlayerManager.instance.PlayerInput.enabled = true;
                    PlayerManager.instance.ChangeControlledPart(PlayerPart.body);
                    PlayerManager.instance.StartCoroutine(doLatter());
                    gameObject.SetActive(false);
                }
            );
        transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
    }

    private IEnumerator doLatter()
    {
        yield return new WaitForSeconds(0.5f);
        bodyScript.canThrowHand = false;
    }
    
    private IEnumerator Dash()
    {
        canDash = false;
        handAnimator.SetBool("IsDashing",true);
        elementRigidbody.linearVelocityX += dashSpeed*direction;
        yield return new WaitForSeconds(dashDuration);
        handAnimator.SetBool("IsDashing",false);
        elementRigidbody.linearVelocityX -= dashSpeed*direction;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnEnable()
    {
        canDash = true;
    }

    private void OnDisable()
    {
        bodyScript.bodyAnimator.SetBool("IsArmless",false);
    }
    
    public override void Die()
    {
        Recall();
    }

    public override void Accroche(CrochetBalance crochet)
    {
        handAnimator.SetBool("IsWalking", false);
        accroche = true;
        currentCrochet = crochet;
        elementRigidbody.simulated = false;
        moveInput = Vector2.zero;
        transform.DOMove(crochet.gameObject.transform.position - new Vector3(0, 0.8f, 0), tempsAccroche)
            .OnComplete(() =>
            {
                gameObject.transform.parent = currentCrochet.transform;
                crochet.moving = true;
            });
    }
    
    public override void Accroche(CrochetPlatform crochet, FallingPlatform fallingPlatform)
    {
        PlayerManager.instance.ChangeControlledPart(PlayerPart.body);
        handAnimator.SetBool("IsWalking", false);
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
        gameObject.transform.parent = null;
        gameObject.transform.eulerAngles = Vector3.zero;
        elementRigidbody.simulated = true;
        accroche = false;
        currentCrochet.StartCoroutine(currentCrochet.Active(elementRigidbody));
        currentCrochet = null;
    }
}
