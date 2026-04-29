using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class BodyData : PlayerData
{
    public float jumpHeight;
    public float launchForce;
    public float coyoteTime;
    public float bufferingTime;
}
