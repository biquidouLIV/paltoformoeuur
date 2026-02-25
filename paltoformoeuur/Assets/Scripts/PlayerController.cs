using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float sprintSpeedMultiplier = 2;
    [SerializeField] private float jumpHeight = 50;
    [SerializeField] private float jumpRaycastSize = 1;

    [SerializeField] private Vector2 rotationInput;
    [SerializeField] private Vector2 rotation;
    
    [SerializeField] private GameObject handPrefab;

    public Rigidbody2D playerRigidbody;
    public PlayerInput playerInput;

    private float sprintSpeed = 1;
    private Vector2 moveInput;
    
    private GameObject aim;
    private bool isAiming;
    
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        transform.Translate(new Vector2(moveInput.x * speed * sprintSpeed * Time.deltaTime,0),Space.World);
        
        if (rotation.x != 0)
        {
            float angle = Mathf.Atan(rotation.y / rotation.x);

            Vector2 rot = new(0f, 0f);
            
            if (rotation.x > 0)
            {
                rot = new (0f, angle * 180 / Mathf.PI + 270);
            }
            else
            {
                rot = new (0f, angle * 180 / Mathf.PI + 90);
            }
        }
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        if (isAiming)
        {
            rotationInput = context.ReadValue<Vector2>();
            if (rotationInput.x + rotationInput.y > 0.1 || rotationInput.x + rotationInput.y < -0.1)
            {
                rotation = rotationInput.normalized;
            }
            moveInput = Vector2.zero;
        }
        else
        {
            rotation = Vector2.zero;
            moveInput = context.ReadValue<Vector2>();
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
        GameObject hand = Instantiate(handPrefab, transform.position, Quaternion.identity);
        HandController script = hand.GetComponent<HandController>();
        script.player = this;
        
        playerRigidbody.linearVelocity = Vector2.zero;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        moveInput = Vector2.zero;

        gameObject.layer = 0;
        playerInput.enabled = false;
        hand.GetComponent<Rigidbody2D>().AddForce(new (rotation.x * 500, rotation.y * 500));
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAiming = true;
        }
        else if (context.canceled && isAiming)
        {
            isAiming = false;
            SpawnHand();
        }
    }
    
}
