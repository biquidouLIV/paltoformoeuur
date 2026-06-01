using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerController : MonoBehaviour
{
    
    
    [Header("Refs")]
        [SerializeField] private PlayerData data;
        [SerializeField] protected GameObject player;

    
    [NonSerialized] public Rigidbody2D elementRigidbody;
    [NonSerialized] public Vector2 moveInput;
    
    protected float sprintSpeed = 1;
    private float speed = 1;
    //protected float sprintSpeedMultiplier = 2;
    
    
    protected BodyController bodyScript;
    
    public abstract void Die();
    public virtual void Init(PlayerData data){}
    
    protected virtual void Start()
    {
        speed = data.speed;
        //sprintSpeedMultiplier = data.sprintSpeedMultiplier;
        Init(data);
        
        elementRigidbody = GetComponent<Rigidbody2D>();
        bodyScript = player.GetComponent<BodyController>();
    }
    
    protected virtual void Update()
    {
        transform.Translate(new Vector2(moveInput.x * speed * sprintSpeed * Time.deltaTime,0),Space.World);
    }
    
    public virtual void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    public void DisableElement()
    {
        elementRigidbody.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;
    }
    
    public virtual void Recall()
    {
        transform.parent = player.transform;
        elementRigidbody.simulated = false;
    }

    public virtual void Accroche(CrochetBalance crochet)
    {
        
    }
    
    public virtual void Accroche(CrochetPlatform crochet, FallingPlatform fallingPlatform)
    {
        
    }
    
    public virtual void Decroche()
    {
        
    }



}
        