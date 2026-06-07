using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Button[] buttonList;
    
    private bool isOpen;

    public void Open()
    {
        foreach (var button in buttonList)
        {
            if (!button.isActivated) return;
        }
        
        if(isOpen) return;
        
        GetComponentInChildren<Animator>().Play("Open");
        GetComponent<BoxCollider2D>().enabled = false;
        isOpen = true;
    }
}
