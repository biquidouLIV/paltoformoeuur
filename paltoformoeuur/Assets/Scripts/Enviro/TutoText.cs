using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TutoText : MonoBehaviour
{
    [SerializeField] private float distanceToSee = 10f;
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        text.color = new Color(255, 255, 25, 0);
    }

    private void Update()
    {
        if (Vector3.Distance(PlayerManager.instance.bodyController.transform.position, transform.position) < distanceToSee)
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
        if(text.color == new Color(255,255,255,0)) return;
        DOTween.To(() => text.color, x => text.color = x, new Color(255,255,255,0), 1);
    }

    private void Show()
    {
        if(text.color == Color.white) return;
        DOTween.To(() => text.color, x => text.color = x, Color.white, 1);
    }
    
    
}
