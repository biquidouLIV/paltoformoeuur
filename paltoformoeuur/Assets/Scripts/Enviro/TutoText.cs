using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TutoText : MonoBehaviour
{
    private enum Type
    {
        player,
        head,
    }

    [SerializeField] private Type playerPart = Type.player;
    [SerializeField] private float distanceToSee = 10f;
    private Color invisible = new (255,255,255,0);

    private PlayerController target;
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        text.color = invisible;

        switch (playerPart)
        {
            case(Type.player):
                target = PlayerManager.instance.bodyController;
                break;
            case(Type.head):
                target = PlayerManager.instance.headController;
                break;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < distanceToSee)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        if(text.color == invisible) return;
        DOTween.To(() => text.color, x => text.color = x, invisible, 1);
    }

    private void Show()
    {
        if(text.color == Color.white) return;
        DOTween.To(() => text.color, x => text.color = x, Color.white, 1);
    }
    
    
}
