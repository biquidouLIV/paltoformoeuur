using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private int indiceCheckpoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enterCP");
        if (playerManager.indiceCheckpoint < indiceCheckpoint)
        {
            playerManager.checkpointTransform = transform.position;
            playerManager.indiceCheckpoint = indiceCheckpoint;
        }
    }
}
