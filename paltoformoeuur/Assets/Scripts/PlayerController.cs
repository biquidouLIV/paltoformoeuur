using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected float speed = 1;
    [SerializeField] protected float sprintSpeedMultiplier = 2;
    [SerializeField] protected GameObject player;
    
    [NonSerialized] public Rigidbody2D elementRigidbody;

    private float sprintSpeed = 1;
    protected Vector2 moveInput;

    protected BodyController playerScript;
    
    private void Start()
    {
        elementRigidbody = GetComponent<Rigidbody2D>();
        playerScript = player.GetComponent<BodyController>();
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
    
    private void DisableElement()
    {
        elementRigidbody.simulated = false;
        transform.parent = player.transform;
        playerScript.elementRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        PlayerManager.instance.OnSelectChange(PlayerManager.PlayerPart.body);
    }
    
    public void Recall(InputAction.CallbackContext context)
    {
        transform.DOMove(player.transform.position, 1).OnComplete(DisableElement);
    }
}
        