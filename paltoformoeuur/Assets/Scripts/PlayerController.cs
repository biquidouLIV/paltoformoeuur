using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float sprintSpeedMultiplier = 2;
    [SerializeField] private float jumpHeight = 50;



    private Rigidbody2D playerRigidbody;

    private float sprintSpeed = 1;
    private Vector2 moveInput;

    
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        transform.Translate(new Vector2(moveInput.x * speed * sprintSpeed * Time.deltaTime,0),Space.World);
    }
    
    
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprintSpeed = sprintSpeedMultiplier;
            Debug.Log("is sprinting");
        }

        if (context.canceled)
        {
            sprintSpeed = 1;
            Debug.Log("is not sprinting");
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && CheckIfGrounded())
        {
            playerRigidbody.AddForce(new Vector2(0,jumpHeight));
        }

        if (context.canceled)
        {
            if (playerRigidbody.linearVelocityY > 0)
            {
                playerRigidbody.linearVelocityY = 0;
            }
        }
    }
    private bool CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down,1,LayerMask.GetMask("Ground"));
        if (hit)
        {
            return true;
        }
            return false;
    }


    


}
