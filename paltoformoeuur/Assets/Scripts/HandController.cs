using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float sprintSpeedMultiplier = 2;
    [SerializeField] private float jumpHeight = 50;
    [SerializeField] private float jumpRaycastSize = 1;
    
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
            handRigidbody.AddForce(new Vector2(0,jumpHeight));
        }

        if (context.canceled)
        {
            if (handRigidbody.linearVelocityY > 0)
            {
                handRigidbody.linearVelocityY = 0;
            }
        }
    }
    private bool CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down * jumpRaycastSize,1,LayerMask.GetMask("Ground"));
        return hit;
    }

    private void DespawnHand()
    {
        player.playerInput.enabled = true;
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
