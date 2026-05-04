using System;
using DG.Tweening;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Door[] doorList;
    private Animator animator;
    [NonSerialized] public bool isActivated;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void OnCollisionEnter2D(Collision2D other)
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
