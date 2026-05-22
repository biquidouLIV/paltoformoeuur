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
    
    
    private Camera testCamera;
    private CinemachineCamera cinemachine;
    private CinemachinePositionComposer cinemachinePositionComposer;
    
    private HeadController head; 
    private BodyController body;
    private PlayerController targetPart;
    
    
    private Vector2 cameraOffset;
    
    private float horizontalDistance;
    private float horizontalSpeed;
    
    private float verticalDistance;
    private float verticalSpeed;
    private float minVelocity;
    private float minDistanceWithGound;
    
    private float bodyCameraFOV;
    private float headCameraFOV ;
    private float FOVTransitionDuration;
    private float targetFOV;

    private Vector3 lastFramePosition;
    private Vector3 targetPosition;
    private Vector3 destination;
    private Vector2 direction;

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

        horizontalDistance = data.horizontalDistance;
        horizontalSpeed = data.horizontalSpeed;

        verticalDistance = data.verticalDistance;
        verticalSpeed = data.verticalSpeed;
        minVelocity = data.minVelocity;
        minDistanceWithGound = data.minDistanceWithGround;

        bodyCameraFOV = data.bodyCameraFOV;
        headCameraFOV = data.headCameraFOV;
        FOVTransitionDuration = data.FOVTransitionDuration;

        defaultOffset = data.defaultOffset;


        testCamera = Camera.current;
        targetPart = body;
        targetPosition = targetPart.transform.position;
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
        lastFramePosition = targetPart.transform.position;
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

