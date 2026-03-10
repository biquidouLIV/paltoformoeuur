using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("paramètres")]
        [SerializeField] protected float speed = 1;
        [SerializeField] protected float sprintSpeedMultiplier = 2;
        [SerializeField] private int recallSpeed;
    
    [Header("Refs")]
        [SerializeField] protected GameObject player;

    
    [NonSerialized] public Rigidbody2D elementRigidbody;
    [NonSerialized] public Vector2 moveInput;
    private float sprintSpeed = 1;
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
        elementRigidbody.linearVelocity = Vector2.zero;
        moveInput = Vector2.zero;
        PlayerManager.instance.OnSelectChange(PlayerManager.PlayerPart.body);
    }
    
    public void Recall(InputAction.CallbackContext context)
    {
        PlayerManager.instance.PlayerInput.enabled = false;
        transform.parent = player.transform;
        elementRigidbody.simulated = false;
        switch (PlayerManager.instance.controlledPart)
        {
            case(PlayerManager.PlayerPart.hand):
                transform.DOLocalMove(PlayerManager.instance.handAnchorPosition, Vector2.Distance(transform.position, player.transform.position) / recallSpeed)
                         .OnComplete(() =>
                             {
                                 playerScript.bodyAnimator.SetBool("IsArmless",false);
                                 DisableElement();
                                 PlayerManager.instance.handOnBody = true;
                                 PlayerManager.instance.PlayerInput.enabled = true;
                             }
                        );
                transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                break;
            
            case(PlayerManager.PlayerPart.head):
                transform.DOLocalMove(PlayerManager.instance.headAnchorPosition, Vector2.Distance(transform.position, player.transform.position) / recallSpeed)
                    .OnComplete(() =>
                        {
                            playerScript.colliderWithHead.enabled = true;
                            playerScript.colliderWithoutHead.enabled = false;
                            playerScript.bodyAnimator.SetBool("IsHeadless",false);
                            DisableElement();
                            PlayerManager.instance.headOnBody = true;
                            PlayerManager.instance.PlayerInput.enabled = true;
                        }
                    );
                transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
                break;
        }
        
    }
}
        