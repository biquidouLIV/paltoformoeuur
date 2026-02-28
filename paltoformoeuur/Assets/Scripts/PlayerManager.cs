using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    [SerializeField] private BodyController bodyController;
    [SerializeField] private HandController handController;
    [SerializeField] private PlayerPart selectedPart;
    [SerializeField] private PlayerPart controlledPart;
    [SerializeField] private Camera camera;

    private bool handOnBody = true;
    private bool headOnBody = true;

    private int numberOfBodyPart = 3;
    
    private enum PlayerPart
    {
        body = 0,
        hand = 1,
        head = 2
    }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
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
        if (!context.started) return;
        selectedPart += 1;
        if (selectedPart.Equals(numberOfBodyPart))
        {
            selectedPart = 0;
        }
        
        switch (selectedPart)
        {
            case PlayerPart.body:
                controlledPart = selectedPart;
                //Set la camera sur le corps
                break;
            case PlayerPart.hand:
                if (handOnBody)
                {
                    //Afficher la main comme élément sélectionné
                }
                else
                {
                    controlledPart = selectedPart;
                    //Set la camera sur la main
                }
                break;
            case PlayerPart.head:
                if (headOnBody)
                {
                    //Afficher la tete comme élément sélectionné
                }
                else
                {
                    controlledPart = selectedPart;
                    //Set la camera sur la tete
                }
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
        selectedPart = PlayerPart.hand;
        controlledPart = PlayerPart.hand;
    }
}
