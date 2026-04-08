using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "Scriptable Objects/CameraData")]
public class CameraData : ScriptableObject
{
    public float bodyCameraFOV = 5f;
    public float headCameraFOV = 10f;
    public float FOVTransitionDuration = 0.2f;
    public Ease ease = Ease.OutCubic;
}
