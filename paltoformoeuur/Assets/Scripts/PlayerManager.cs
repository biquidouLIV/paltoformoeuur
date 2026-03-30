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
    [SerializeField] public PlayerPart selectedPart;
    [SerializeField] public PlayerPart controlledPart;
    
    [NonSerialized] public Vector3 handAnchorPosition;
    [NonSerialized] public Vector3 headAnchorPosition;

    [SerializeField] public PlayerInput PlayerInput;
    [SerializeField] public TMP_Text textElementSelectionne;
    
    public bool handOnBody = true;
    public bool headOnBody = true;

    public Vector3 checkpointTransform;
    public int indiceCheckpoint;

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
                textElementSelectionne.text = "body";
                controlledPart = selectedPart;
                break;
            case PlayerPart.hand:
                bodyController.moveInput = Vector2.zero;
                textElementSelectionne.text = "hand";

                if (handOnBody)
                {
                    controlledPart = PlayerPart.body;
                }
                else
                {
                    controlledPart = selectedPart;
                }
                break;
            case PlayerPart.head:
                handController.moveInput = Vector2.zero;
                textElementSelectionne.text = "head";
                if (headOnBody)
                {
                    controlledPart = PlayerPart.body;
                }
                else
                {
                    controlledPart = selectedPart;
                }
                break;
            default:
                Debug.LogError("No selected part");
                break;
        }
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
                handController.Recall();
                break;
            case PlayerPart.head:
                headController.Recall();
                break;
            default:
                Debug.LogError("No controlled part");
                break;
        }
    }
}
