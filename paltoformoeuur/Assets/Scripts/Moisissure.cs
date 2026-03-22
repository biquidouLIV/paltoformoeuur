using UnityEngine;

public class Moisissure : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enterMoisi");
        playerManager.Respawn();
    }
}
