using System;
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
/*
    [SerializeField] private CameraData data;
    [SerializeField] private CinemachineCamera camera;
    */
    [SerializeField] private Camera testCamera;
    [SerializeField] private HeadController head;
    [SerializeField] private BodyController body;

    
     private float bodyCameraFOV;
     private float headCameraFOV;
     private float FOVTransitionDuration;
     private Ease ease;
        
     private float targetFOV;
     private PlayerController targetPart;


     [SerializeField] private float multiplier;
     [SerializeField] private float speed = 1;


     private Vector3 lastFramePosition;
     private Vector3 cameraOffset;
     
     private void Start()
     {
         lastFramePosition = transform.position;
         cameraOffset = transform.position;
         targetFOV = bodyCameraFOV;
         targetPart = body;
         /*
         bodyCameraFOV = data.bodyCameraFOV;
         headCameraFOV = data.headCameraFOV;
         FOVTransitionDuration = data.FOVTransitionDuration;
         ease = data.ease;
         */


     }
     private void FixedUpdate()
     {
         
         Debug.Log(targetPart.elementRigidbody.linearVelocity);
         Vector3 direction = (transform.position - lastFramePosition).normalized;
         cameraOffset = new Vector3(direction.x * multiplier, direction.y * multiplier,-10);
        

         
         testCamera.transform.DOLocalMove(cameraOffset + new Vector3(0,0,-10), speed);
         lastFramePosition = transform.position;
     }
     
     
    public void ChangeFOV(PlayerPart part)
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
                break;
        }
        /*
        DOTween.To(() => camera.Lens.OrthographicSize, x => camera.Lens.OrthographicSize = x, targetFOV,
                FOVTransitionDuration)
            .SetEase(ease);
            */
    }
}

