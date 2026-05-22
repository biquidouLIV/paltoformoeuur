using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "Scriptable Objects/CameraData")]
public class CameraData : ScriptableObject
{
    public float bodyCameraFOV = 5;
    public float headCameraFOV = 12;
    public float FOVTransitionDuration;
}
