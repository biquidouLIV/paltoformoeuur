using System;
using DG.Tweening;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    
    [SerializeField] private Vector2 arrowDistance;
    [SerializeField] private Vector2 maxDistance;

    [SerializeField] private float scaleDuration = 0.1f;
    
    private Vector2 distance;
    private BodyController body;
    private HandController hand;
    private HeadController head;
    private PlayerController targetPart;

    private void Start()
    {
        arrow.transform.localScale = new Vector3(0, 0, 0);
        body = PlayerManager.instance.bodyController;
        head = PlayerManager.instance.headController;
        hand = PlayerManager.instance.handController;
    }

    private void Update()
    {
        switch (PlayerManager.instance.controlledPart)
        {
            case(PlayerPart.body):
                targetPart = body;
                break;
            case(PlayerPart.hand):
                targetPart = hand;
                break;
        }
        
        
        
        distance.x = Mathf.Abs(head.transform.position.x - targetPart.transform.position.x);
        distance.y = Mathf.Abs(head.transform.position.y - targetPart.transform.position.y);
        
        
        if(!PlayerManager.instance.headOnBody)
        {
            if (distance.x >= maxDistance.x || distance.y >= maxDistance.y)
            {
               Show();
            }
            else
            {
                Hide();
            }
        }
        else
        {
            Hide();
        }




        Vector2 destination = targetPart.transform.position;
        destination.x = Mathf.Clamp(destination.x, head.transform.position.x - arrowDistance.x, head.transform.position.x + arrowDistance.x);
        destination.y = Mathf.Clamp(destination.y, head.transform.position.y - arrowDistance.y, head.transform.position.y + arrowDistance.y);
        
        arrow.transform.DOMove(destination, 0.1f);
        
        float rotation = Mathf.Acos((targetPart.transform.position.x - head.transform.position.x)/Vector3.Distance(targetPart.transform.position,head.transform.position)) * 180/Mathf.PI;
        if (targetPart.transform.position.y < head.transform.position.y)
        {
            rotation = -rotation;
        }
        arrow.transform.DORotate(new Vector3(0, 0, rotation), 0.1f);

    }

    private void Show()
    {
        arrow.transform.DOScale(new Vector3(1,1,1),scaleDuration);
    }

    private void Hide()
    {
        arrow.transform.DOScale(new Vector3(0,0,0),scaleDuration);
    }
    
}
