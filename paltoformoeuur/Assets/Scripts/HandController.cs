using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float sprintSpeedMultiplier = 2;
    [SerializeField] private float dashSpeed = 50;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 3.0f;
    [SerializeField] private float jumpRaycastSize = 1;
    
    

    private bool canDash = true;
    private int direction = 1;
    
    private float sprintSpeed = 1;
    private Vector2 moveInput;

    public Rigidbody2D handRigidbody;
    
    public PlayerController player;

    private void Start()
    {
        handRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector2(moveInput.x * speed * sprintSpeed * Time.deltaTime,0),Space.World);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput.x > 0)
        {
            direction = 1;
        }
        else if(moveInput.x < 0)
        {
            direction = -1;
        }

    }
    
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprintSpeed = sprintSpeedMultiplier;
        }

        if (context.canceled)
        {
            sprintSpeed = 1;
        }
        Debug.Log(moveInput);
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
        handRigidbody.linearVelocityX = dashSpeed*direction;
        yield return new WaitForSeconds(dashDuration);
        handRigidbody.linearVelocityX /= 5;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }
    
    
    private void DespawnHand()
    {
        transform.DOMove(player.transform.position, 1)
            .OnComplete(DisableHand);

    }

    private void DisableHand()
    {
        GetComponent<PlayerInput>().enabled = false;
        player.playerInput.enabled = true;
        
        handRigidbody.simulated = false;
        player.playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
 
    }


    public void OnSpawnHand(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DespawnHand();
        }
    }


}
