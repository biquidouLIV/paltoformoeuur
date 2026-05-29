using UnityEngine;

// ptet faire des dossiers, là c'est un peu le bronx tout directement dans le dossier scripts

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class BodyData : PlayerData
{
    public float jumpHeight;
    public float launchForce;
    public float coyoteTime;
    public float bufferingTime;
}
