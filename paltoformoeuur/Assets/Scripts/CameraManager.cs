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

    [SerializeField] private CameraData data;
    [SerializeField] private CinemachineCamera camera;

     private float bodyCameraFOV;
     private float headCameraFOV;
     private float FOVTransitionDuration;
     private Ease ease;
        
     private float targetFOV;

    private void Start()
    {
        bodyCameraFOV = data.bodyCameraFOV;
        headCameraFOV = data.headCameraFOV;
        FOVTransitionDuration = data.FOVTransitionDuration;
        ease = data.ease;
    }

    public void ChangeFOV(PlayerPart part)
    {
        switch (part)
        {
            case PlayerPart.body:
                targetFOV = bodyCameraFOV;
                break;
            case PlayerPart.head:
                targetFOV = headCameraFOV;
                break;
        }

        DOTween.To(() => camera.Lens.OrthographicSize, x => camera.Lens.OrthographicSize = x, targetFOV,
                FOVTransitionDuration)
            .SetEase(ease);
    }
}

