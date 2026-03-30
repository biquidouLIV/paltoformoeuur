using System;
using DG.Tweening;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private Vector3 doorMove = new Vector3(0,3,0);
    [SerializeField] private float doorOpeningTime;

    private bool isOpen;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(isOpen) return;
        
        door.transform.DOMove(door.transform.position + doorMove, doorOpeningTime)
            .SetEase(Ease.InOutCubic);
        isOpen = true;

    }
}
