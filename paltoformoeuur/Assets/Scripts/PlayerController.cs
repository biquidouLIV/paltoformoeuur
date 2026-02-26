using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float sprintSpeedMultiplier = 2;
    [SerializeField] private float jumpHeight = 50;
    [SerializeField] private float jumpRaycastSize = 1;
    [SerializeField] private float jumpStopDelay = 0.2f;

    [SerializeField] private Camera camera;
    [SerializeField] private GameObject hand;
    [SerializeField] private HandController handController;
    
    public Transform handTransform;
    public Rigidbody2D playerRigidbody;
    public PlayerInput playerInput;

    private float sprintSpeed = 1;
    private Vector2 moveInput;

    
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        handTransform = hand.transform;
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
        }

        if (context.canceled)
        {
            sprintSpeed = 1;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && CheckIfGrounded())
        {
            StopAllCoroutines();
            playerRigidbody.AddForce(new Vector2(0,jumpHeight));
        }

        if (context.canceled)
        {
            if (playerRigidbody.linearVelocityY > 0)
            {
                playerRigidbody.linearVelocityY /= 2;
            }
        }
    }
    private bool CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, jumpRaycastSize, ~LayerMask.GetMask("Player"));
        return hit;
    }

    public void OnSpawnHand(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            SpawnHand();
        }
    }


    private void SpawnHand()
    {
        hand.GetComponent<PlayerInput>().enabled = true;
        playerInput.enabled = false;

        handController.handRigidbody.simulated = true; 
        playerRigidbody.linearVelocity = Vector2.zero;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        moveInput = Vector2.zero;
        
    }
    
}
