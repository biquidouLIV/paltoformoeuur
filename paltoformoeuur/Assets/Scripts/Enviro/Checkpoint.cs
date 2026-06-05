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
            SoundManager.instance.PlaySound(SoundManager.instance.triggerCheckpoint);
            PlayerManager.instance.checkpointTransform = transform.position;
            PlayerManager.instance.indiceCheckpoint = indiceCheckpoint;
        }
        
    }
}
