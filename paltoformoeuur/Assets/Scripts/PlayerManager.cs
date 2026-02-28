using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    [SerializeField] private BodyController bodyController;
    [SerializeField] private HandController handController;
    [SerializeField] private HeadController headController;
    [SerializeField] public PlayerPart selectedPart;
    [SerializeField] public PlayerPart controlledPart;

    private bool handOnBody = true;
    private bool headOnBody = true;

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
        if ((int)selectedPart == (numberOfBodyPart))
        {
            selectedPart = 0;
        }
        Debug.Log(selectedPart);
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
                if (handOnBody)
                {
                    //Afficher la main comme élément sélectionné
                }
                else
                {
                    controlledPart = selectedPart;
                    CameraManager.instance.SetOnHand();
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
                    CameraManager.instance.SetOnHead();
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
