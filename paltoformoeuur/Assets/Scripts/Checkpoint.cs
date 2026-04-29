using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int indiceCheckpoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Body"))
        {
            return;
        }
        if (PlayerManager.instance.indiceCheckpoint < indiceCheckpoint)
        {
            PlayerManager.instance.checkpointTransform = transform.position;
            PlayerManager.instance.indiceCheckpoint = indiceCheckpoint;
        }
        
    }
}
