using System;
using DG.Tweening;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private Vector2 arrowDistance;
    [SerializeField] private Vector2 maxDistance;
    
    private Vector2 distance;
    private BodyController body;
    private HeadController head;
    
    
    



    private void Start()
    {
        arrow.SetActive(false);
        body = PlayerManager.instance.bodyController;
        head = PlayerManager.instance.headController;
    }

    private void Update()
    {
        distance.x = Mathf.Abs(head.transform.position.x - body.transform.position.x);
        distance.y = Mathf.Abs(head.transform.position.y - body.transform.position.y);
        
        
        if(!PlayerManager.instance.headOnBody)
        {
            if (distance.x >= maxDistance.x || distance.y >= maxDistance.y)
            {
                arrow.SetActive(true);
            }
            else
            {
                arrow.SetActive(false);
            }
        }
        else
        {
            arrow.SetActive(false);
        }
        
        
        
        Vector2 destination = body.transform.position;
        destination.x = Mathf.Clamp(destination.x, head.transform.position.x - arrowDistance.x, head.transform.position.x + arrowDistance.x);
        destination.y = Mathf.Clamp(destination.y, head.transform.position.y - arrowDistance.y, head.transform.position.y + arrowDistance.y);
        
        arrow.transform.DOMove(destination, 0.1f);

        
        float rotation = Mathf.Acos((body.transform.position.x - head.transform.position.x)/Vector3.Distance(body.transform.position,head.transform.position)) * 180/Mathf.PI;
        if (body.transform.position.y < head.transform.position.y)
        {
            rotation = -rotation;
        }
        
        
        Debug.Log(rotation);
        arrow.transform.DORotate(new Vector3(0, 0, rotation), 0.1f);

    }
}
