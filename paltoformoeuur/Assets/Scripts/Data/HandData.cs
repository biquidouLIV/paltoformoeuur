using UnityEngine;

[CreateAssetMenu(fileName = "HandData", menuName = "Scriptable Objects/HandData")]
public class HandData : PlayerData
{
    public float dashSpeed = 50;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3.0f;
    public int recallSpeed = 20;
}
