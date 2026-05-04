using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Button[] buttonList;
    [SerializeField] private Vector3 doorMove = new Vector3(0,3,0);
    [SerializeField] private float doorOpeningTime = 1f;
    [SerializeField] private Ease ease = Ease.OutCubic;
    
    private bool isOpen;

    public void Open()
    {
        foreach (var button in buttonList)
        {
            if (!button.isActivated) return;
        }
        
        if(isOpen) return;
        
        transform.DOMove(transform.position + doorMove, doorOpeningTime)
            .SetEase(ease);
        isOpen = true;
    }
}
