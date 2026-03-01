using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    [SerializeField] private BodyController bodyController;
    [SerializeField] public HandController handController;
    [SerializeField] private HeadController headController;
    [SerializeField] public PlayerPart selectedPart;
    [SerializeField] public PlayerPart controlledPart;
    
    [SerializeField] public Vector3 handAnchorPosition;
    [SerializeField] public Vector3 headAnchorPosition;

    [SerializeField] public PlayerInput PlayerInput;
    
    public bool handOnBody = true;
    public bool headOnBody = true;

    private int numberOfBodyPart = 3;
    
    public enum PlayerPart
    {
        body = 0,
        hand = 1,
        head = 2
    }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    private void Start()
    {
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
                handController.OnMove(context);
                break;
            case PlayerPart.head:
                break;
            default:
                Debug.LogError("No controlled part");
                break;
        }
    }

    public void OnSelectChange(InputAction.CallbackContext context)
    {
        if(bodyController.isAiming) return;
        
        if (context.performed)
        {
            selectedPart += 1;
            if ((int)selectedPart == numberOfBodyPart)
            {
                selectedPart = 0;
            }
        }
        CheckControlledPart();
    }

    public void OnSelectChange(PlayerPart playerPart)
    {
        selectedPart = playerPart;
        CheckControlledPart();
    }

    private void CheckControlledPart()
    {
        switch (selectedPart)
        {
            case PlayerPart.body:
                controlledPart = selectedPart;
                CameraManager.instance.SetOnBody();
                break;
            case PlayerPart.hand:
                bodyController.moveInput = Vector2.zero;
                if (handOnBody)
                {
                    //Afficher la main comme élément sélectionné
                    controlledPart = PlayerPart.body;
                    CameraManager.instance.SetOnBody();
                }
                else
                {
                    
                    controlledPart = selectedPart;
                    CameraManager.instance.SetOnHand();
                }
                Debug.Log(handOnBody);
                break;
            case PlayerPart.head:
                handController.moveInput = Vector2.zero;
                if (headOnBody)
                {
                    //Afficher la tete comme élément sélectionné
                    controlledPart = PlayerPart.body;
                    CameraManager.instance.SetOnBody();
                }
                else
                {
                    
                    controlledPart = selectedPart;
                    CameraManager.instance.SetOnHead();
                }
                break;
            default:
                Debug.LogError("No selected part");
                break;
        }
        Debug.Log(controlledPart);
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
            case PlayerPart.head:
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
            case PlayerPart.head:
                break;
            default:
                Debug.LogError("No controlled part");
                break;
        }
    }

    public void EnableHand()
    {
        handOnBody = false;
        OnSelectChange(PlayerPart.hand);
    }
    
    public void EnableHead()
    {
        headOnBody = false;
        OnSelectChange(PlayerPart.head);
    }

    public void OnRecall(InputAction.CallbackContext context)
    {
        switch (controlledPart)
        {
            case PlayerPart.body:
                break;
            case PlayerPart.hand:
                handController.Recall(context);
                break;
            case PlayerPart.head:
                headController.Recall(context);
                break;
            default:
                Debug.LogError("No controlled part");
                break;
        }
    }
}
