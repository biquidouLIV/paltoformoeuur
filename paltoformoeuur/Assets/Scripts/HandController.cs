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
    private bool accroche = false;
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

    private void Update()
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
    
    public override void OnSprint(InputAction.CallbackContext context)
    {
        return;
    }

    //ca s'appelle jump mais c'est un dash 
    public void OnJump(InputAction.CallbackContext context) 
    {
        if (context.performed && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public override void Recall()
    {
        base.Recall();
        transform.DOLocalMove(PlayerManager.instance.handAnchorPosition, Vector2.Distance(transform.position, player.transform.position) / recallSpeed)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
                {
                    accroche = false;
                    currentCrochet = null;
                    bodyScript.bodyAnimator.SetBool("IsArmless",false);
                    DisableElement();
                    PlayerManager.instance.handOnBody = true;
                    PlayerManager.instance.PlayerInput.enabled = true;
                    PlayerManager.instance.ChangeControlledPart(PlayerPart.body);
                }
            );
        transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
		gameObject.SetActive(false);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        handAnimator.SetBool("IsDashing",true);
        elementRigidbody.linearVelocityX = dashSpeed*direction;
        yield return new WaitForSeconds(dashDuration);
        handAnimator.SetBool("IsDashing",false);
        elementRigidbody.linearVelocityX = 0;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public override void Die()
    {
        Recall();
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
