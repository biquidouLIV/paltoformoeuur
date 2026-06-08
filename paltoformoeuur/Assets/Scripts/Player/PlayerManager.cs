using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [Header("Slow Motion")]
    public bool slowMo;
    public float slowMoValue;
    [Header("Pas touche GD !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")]
    public BodyController bodyController;
    public HandController handController;
    public HeadController headController;
    public PlayerPart controlledPart;
    
    [NonSerialized] public Vector3 handAnchorPosition;
    [NonSerialized] public Vector3 headAnchorPosition;
    
    [SerializeField] public PlayerInput PlayerInput;
    
    public GameObject flameHead;
    
    public bool handOnBody = true;
    public bool headOnBody = true;

    public Vector3 checkpointTransform;
    public int indiceCheckpoint;
    
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        if (PlayerPrefs.GetInt("musicVolume") == 1) slowMo = true;
        else slowMo = false;
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

    /*public void OnSprint(InputAction.CallbackContext context)
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
    }*/
    
    public void ActiveUnactiveSlowMo(bool isSlowMo)
    {
        slowMo = isSlowMo;
        if (slowMo) PlayerPrefs.SetInt("slowMo", 1);
        else PlayerPrefs.SetInt("slowMo", 0);
    }

    public void EnableHand()
    {
        handOnBody = false;
        ChangeControlledPart(PlayerPart.hand);
    }
    
    public void EnableHead()
    {
        headOnBody = false;
        flameHead.SetActive(true);
    }

    public void OnRecallHead()
    {
        headController.Recall();
    }
    
    public void OnRecallHand()
    {
        handController.Recall();
    }
    
    
    //UI

    #region UI

    public void Pause(InputAction.CallbackContext context)
    {
        UIManager.instance.Pause(context);
    }

    public void NextTab(InputAction.CallbackContext context)
    {
        UIManager.instance.NextTab(context);
    }
    public void PreviousTab(InputAction.CallbackContext context)
    {
        UIManager.instance.PreviousTab(context);
    }

    public void GoBack(InputAction.CallbackContext context)
    {
        UIManager.instance.GoBack(context);
    }

    #endregion
}