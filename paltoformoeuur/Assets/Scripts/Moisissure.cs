using UnityEngine;

public class Moisissure : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enterMoisi");
        PlayerManager.instance.Respawn();
    }
}
