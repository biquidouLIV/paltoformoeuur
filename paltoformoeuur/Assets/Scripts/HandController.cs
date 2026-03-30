using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : PlayerController
{
    [Header("paramètres")]
        [SerializeField] private float dashSpeed = 50;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 3.0f;
        [SerializeField] private int recallSpeed;

    [Header("Refs")]
        [SerializeField] public Animator handAnimator;
        
    private bool canDash = true;
    private bool accroche = false;
    private Crochet currentCrochet;
    private int direction = 1;

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
            .OnComplete(() =>
                {
                    accroche = false;
                    currentCrochet = null;
                    playerScript.bodyAnimator.SetBool("IsArmless",false);
                    DisableElement();
                    PlayerManager.instance.handOnBody = true;
                    PlayerManager.instance.PlayerInput.enabled = true;
                }
            );
        transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
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
        PlayerManager.instance.OnSelectChange(PlayerManager.PlayerPart.hand);
        Recall();
    }

    public void Accroche(Crochet crochet)
    {
        accroche = true;
        currentCrochet = crochet;
        elementRigidbody.simulated = false;
        transform.position = crochet.gameObject.transform.position - new Vector3(0,0.8f,0);
    }
}
