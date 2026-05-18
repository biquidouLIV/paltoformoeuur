using System;
using DG.Tweening;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Door[] doorList;
    [SerializeField] private Animator animator;
    [NonSerialized] public bool isActivated;




    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isActivated)return;
        isActivated = true;

        animator.Play("Activation");
        foreach (var door in doorList)
        {
            door.Open();
        }
    }
}
