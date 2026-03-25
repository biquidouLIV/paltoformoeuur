using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : PlayerController
{
    [Header("paramètres")]
        [SerializeField] private float dashSpeed = 50;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 3.0f;

    private bool canDash = true;
    private int direction = 1;
    
    public override void OnMove(InputAction.CallbackContext context)
    {
        base.OnMove(context);
        if (moveInput.x > 0)
        {
            direction = 1;
        }
        else if(moveInput.x < 0)
        {
            direction = -1;
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
