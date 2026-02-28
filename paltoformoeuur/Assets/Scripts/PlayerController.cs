using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerController : MonoBehaviour
{
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float sprintSpeedMultiplier = 2;
    
    [NonSerialized] public Rigidbody2D elementRigidbody;

    private float sprintSpeed = 1;
    protected Vector2 moveInput;
    

    public abstract void OnJump(InputAction.CallbackContext context);
    
    private void Start()
    {
        elementRigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        transform.Translate(new Vector2(moveInput.x * speed * sprintSpeed * Time.deltaTime,0),Space.World);
    }
    
    public virtual void OnMove(InputAction.CallbackContext context)
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
}
        