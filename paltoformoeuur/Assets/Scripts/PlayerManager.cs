using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [Header("Pas touche GD !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")]
    [SerializeField] private BodyController bodyController;
    [SerializeField] public HandController handController;
    [SerializeField] private HeadController headController;
    [SerializeField] public PlayerPart controlledPart;
    
    [NonSerialized] public Vector3 handAnchorPosition;
    [NonSerialized] public Vector3 headAnchorPosition;

    [SerializeField] public PlayerInput PlayerInput;
    
    public bool handOnBody = true;
    public bool headOnBody = true;

    public Vector3 checkpointTransform;
    public int indiceCheckpoint;

    private int numberOfBodyPart = 3;
    

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    private void Start()
    {
        checkpointTransform = transform.position;
        indiceCheckpoint = 0;
        handAnchorPosition = handController.gameObject.transform.localPosition;
        headAnchorPosition = headController.gameObject.transform.localPosition;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        switch (controlledPart)
        {
            case PlayerPart.body:
                bodyController.OnMove(context);
                break;
            case PlayerPart.hand:
                if (bodyController.isAiming)
                {
                    bodyController.OnMove(context);
                }
                else
                {
                    handController.OnMove(context);
                }
                break;
            default:
                Debug.LogError("No controlled part");
                break;
        }
    }

    public void ChangeControlledPart(PlayerPart playerPart)
    {
        controlledPart = playerPart;
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        switch (controlledPart)
        {
            case PlayerPart.body:
                bodyController.OnJump(context);
                break;
            case PlayerPart.hand:
                handController.OnJump(context);
                break;
            default:
                Debug.LogError("No controlled part");
                break;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        switch (controlledPart)
        {
            case PlayerPart.body:
                bodyController.OnSprint(context);
                break;
            case PlayerPart.hand:
                handController.OnSprint(context);
                break;
            default:
                Debug.LogError("No controlled part");
                break;
        }
    }

    public void EnableHand()
    {
        handOnBody = false;
        ChangeControlledPart(PlayerPart.hand);
    }
    
    public void EnableHead()
    {
        headOnBody = false;
    }

    public void OnRecallHead(InputAction.CallbackContext context)
    {
        headController.Recall();
    }
    
    public void OnRecallHand(InputAction.CallbackContext context)
    {
        handController.Recall();
    }
}