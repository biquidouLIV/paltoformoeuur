using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "Scriptable Objects/CameraData")]
public class CameraData : ScriptableObject
{
    public float horizontalDistance;
    public float horizontalSpeed;
    
    public float verticalSpeed = 1;
    public float verticalDistance = 8;
    public float minVelocity;
    public float minDistanceWithGround;
    
    public float bodyCameraFOV = 5;
    public float headCameraFOV = 12;
    public float FOVTransitionDuration;

    public Vector2 defaultOffset;
}
