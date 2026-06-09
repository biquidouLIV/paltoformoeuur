using System;
using UnityEngine;

public class ChangeTargetOffset : MonoBehaviour
{
    [SerializeField] private int yOffsetChange;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Body"))
        {
            Debug.Log("offsetChange");
            CameraManager.instance.ChangeOffset(yOffsetChange);
        }
    }
}
