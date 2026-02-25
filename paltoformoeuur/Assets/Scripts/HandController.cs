using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float sprintSpeedMultiplier = 2;
    [SerializeField] private float dashLength = 50;
    [SerializeField] private float dashCooldown = 3.0f;
    [SerializeField] private float dashStopDelay = 0.3f;
    [SerializeField] private float jumpRaycastSize = 1;

    private bool canDash = true;
    private int direction = 1;
    
    private float sprintSpeed = 1;
    private Vector2 moveInput;

    private Rigidbody2D handRigidbody;
    
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

        Debug.Log(direction);
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
    }
    
    
    //ca s'appelle jump mais c'est un dash 
    public void OnJump(InputAction.CallbackContext context) 
    {
        if (context.performed && canDash)
        {
            handRigidbody.AddForce(new Vector2(dashLength*direction,0));
            canDash = false;
            StartCoroutine(DashCooldown());
            StartCoroutine(DashStop());
        }
        
    }
    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator DashStop()
    {
        yield return new WaitForSeconds(dashStopDelay);
        handRigidbody.linearVelocityX /= 2;
    }
    
    
    private void DespawnHand()
    {
        player.playerInput.enabled = true;
        player.gameObject.layer = 6;
        player.playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        Destroy(gameObject);
    }
    
    public void OnSpawnHand(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DespawnHand();
        }
    }


}
