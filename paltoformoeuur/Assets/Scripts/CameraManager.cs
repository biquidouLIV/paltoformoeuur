using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private Vector2 defaultOffset;
    
    
    [SerializeField] private CameraData data;
    [SerializeField] private Ease horizontalEase;
    [SerializeField] private Ease verticalEase;
    
    
    private CinemachineCamera cinemachine;
    private CinemachinePositionComposer cinemachinePositionComposer;
    
    private HeadController head; 
    private BodyController body;
    private PlayerController targetPart;
    
    private float bodyCameraFOV;
    private float headCameraFOV ;
    private float FOVTransitionDuration;
    private float targetFOV;


    private Vector3 defaultTargetOffset;
    private float defaultLookAheadTime;
    
     private void Start()
     {
        cinemachine = GetComponent<CinemachineCamera>();
        cinemachinePositionComposer = GetComponent<CinemachinePositionComposer>();
        defaultTargetOffset = cinemachinePositionComposer.TargetOffset;
        defaultLookAheadTime = cinemachinePositionComposer.Lookahead.Time;
        
        
        body = PlayerManager.instance.bodyController;
        head = PlayerManager.instance.headController;
        cinemachine.Follow = head.transform;
        

        bodyCameraFOV = data.bodyCameraFOV;
        headCameraFOV = data.headCameraFOV;
        FOVTransitionDuration = data.FOVTransitionDuration;
        
        
        targetPart = body;
        ChangeTarget(PlayerPart.body);
         
     }
     
    public void ChangeTarget(PlayerPart part)
    {
        switch (part)
        {
            case PlayerPart.body:
                targetFOV = bodyCameraFOV;
                targetPart = body;

                break;
            case PlayerPart.head:
                targetFOV = headCameraFOV;
                targetPart = head;
                cinemachinePositionComposer.TargetOffset = Vector3.zero;
                cinemachinePositionComposer.Lookahead.Enabled = false;
                break;
        }
        DOTween.To(() => cinemachine.Lens.OrthographicSize, x => cinemachine.Lens.OrthographicSize = x, targetFOV, FOVTransitionDuration);
    }

    public void CameraOnRecallHead()
    {
        DOTween.To(() => cinemachinePositionComposer.TargetOffset, x => cinemachinePositionComposer.TargetOffset = x, defaultTargetOffset, 1);
        cinemachinePositionComposer.Lookahead.Enabled = true;
    }
    
    
    
    public IEnumerator CameraOnRespawn()
    {
        cinemachinePositionComposer.Lookahead.Time = 0;
        yield return new WaitForSeconds(3);
        cinemachinePositionComposer.Lookahead.Time = defaultLookAheadTime;
    }


}

