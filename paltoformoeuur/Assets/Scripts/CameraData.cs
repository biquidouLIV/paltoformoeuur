using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "Scriptable Objects/CameraData")]
public class CameraData : ScriptableObject
{
    public float horizontalDistance = 7;
    public float verticalDistance = 7;
    public float speed = 2;
    
    public float bodyCameraFOV = 5;
    public float headCameraFOV = 12;
    public float FOVTransitionDuration;
}
