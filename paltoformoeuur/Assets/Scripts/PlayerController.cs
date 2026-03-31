using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerController : MonoBehaviour
{
    [Header("paramètres")]
        [SerializeField] protected float speed = 1;
        [SerializeField] protected float sprintSpeedMultiplier = 2;
    
    [Header("Refs")]
        [SerializeField] protected GameObject player;

    
    [NonSerialized] public Rigidbody2D elementRigidbody;
    [NonSerialized] public Vector2 moveInput;
    private float sprintSpeed = 1;
    protected BodyController bodyScript;
    
    public abstract void Die();
    
    protected virtual void Start()
    {
        elementRigidbody = GetComponent<Rigidbody2D>();
        bodyScript = player.GetComponent<BodyController>();
    }
    
    private void FixedUpdate()
    {
        transform.Translate(new Vector2(moveInput.x * speed * sprintSpeed * Time.deltaTime,0),Space.World);
    }
    
    public virtual void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    public virtual void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bodyScript.bodyAnimator.SetBool("IsSprinting",true);
            sprintSpeed = sprintSpeedMultiplier;
        }

        if (context.canceled)
        {
            bodyScript.bodyAnimator.SetBool("IsSprinting",false);
            sprintSpeed = 1;
        }
    }
    
    public void DisableElement()
    {
        elementRigidbody.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;
    }
    
    public virtual void Recall()
    {
        PlayerManager.instance.PlayerInput.enabled = false;
        transform.parent = player.transform;
        elementRigidbody.simulated = false;
    }

    public virtual void Accroche(Crochet crochet, FallingPlatform fallingPlatform)
    {
        
    }
}
        