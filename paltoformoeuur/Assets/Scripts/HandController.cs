using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : PlayerController
{
    [Header("paramètres")]
        [SerializeField] private float dashSpeed = 50;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 3.0f;

    [Header("Refs")]
        [SerializeField] public Animator handAnimator;
        
    private bool canDash = true;
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
        base.OnSprint(context);
        if (context.performed)
        {
            handAnimator.SetBool("IsSprinting",true);
        }

        if (context.canceled)
        {
            handAnimator.SetBool("IsSprinting",false);
        }
    }



    //ca s'appelle jump mais c'est un dash 
    public void OnJump(InputAction.CallbackContext context) 
    {
        if (context.performed && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        handAnimator.SetTrigger("IsDashing");
        elementRigidbody.linearVelocityX = dashSpeed*direction;
        yield return new WaitForSeconds(dashDuration);
        elementRigidbody.linearVelocityX = 0;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public override void Die()
    {
        PlayerManager.instance.OnSelectChange(PlayerManager.PlayerPart.hand);
        Recall();
    }
}
