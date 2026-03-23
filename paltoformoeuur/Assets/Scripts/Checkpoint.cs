using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int indiceCheckpoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enterCP");
        if (PlayerManager.instance.indiceCheckpoint < indiceCheckpoint)
        {
            PlayerManager.instance.checkpointTransform = transform.position;
            PlayerManager.instance.indiceCheckpoint = indiceCheckpoint;
        }
        
    }
}
