using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CrochetBalance : Crochet
{
    [SerializeField] private float delayOnLeaving = 1;
    [SerializeField] private float strength = 1;
    [SerializeField] private Ease rotationEase;
    [SerializeField] private float timeForOneRotation;
    [SerializeField] private float speedForFirstRotation = 2;
    private bool isAvailable = true;
    private PlayerController playerController;
    private GameObject parent;
    public bool moving;

    private void Start()
    {
        parent = gameObject.transform.parent.gameObject;
    }

    public void StartRotation(bool goLeft)
    {
        moving = true;
        
        if (!goLeft)
        {
            parent.transform.DORotate(new Vector3(0, 0, 60), timeForOneRotation/speedForFirstRotation)
                .SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    if(PlayerManager.instance.bodyController.accroche) PlayerManager.instance.bodyController.bodyAnimator.SetTrigger("ChangeBalancingSide");
                    if(PlayerManager.instance.handController.accroche) PlayerManager.instance.handController.handAnimator.SetTrigger("ChangeBalancingSide");
                    DoRotation(true);
                });
        }
        else
        {
            parent.transform.DORotate(new Vector3(0, 0, 310), timeForOneRotation/speedForFirstRotation)
                .SetEase(Ease.OutCubic).OnComplete(() =>
                {
                    if(PlayerManager.instance.bodyController.accroche) PlayerManager.instance.bodyController.bodyAnimator.SetTrigger("ChangeBalancingSide");
                    if(PlayerManager.instance.handController.accroche) PlayerManager.instance.handController.handAnimator.SetTrigger("ChangeBalancingSide");
                    DoRotation(false);
                });
        }
}

    public void DoRotation(bool left)
    {
        if (moving)
        {
            if (!left)
            {
                parent.transform.DORotate(new Vector3(0, 0, 60), timeForOneRotation)
                    .SetEase(rotationEase).OnComplete(() =>
                    {
                        if(PlayerManager.instance.bodyController.accroche) PlayerManager.instance.bodyController.bodyAnimator.SetTrigger("ChangeBalancingSide");
                        if(PlayerManager.instance.handController.accroche) PlayerManager.instance.handController.handAnimator.SetTrigger("ChangeBalancingSide");
                        DoRotation(true);
                    });
            }
            else
            {
                parent.transform.DORotate(new Vector3(0, 0, 310), timeForOneRotation)
                    .SetEase(rotationEase).OnComplete(() =>
                    {
                        if(PlayerManager.instance.bodyController.accroche) PlayerManager.instance.bodyController.bodyAnimator.SetTrigger("ChangeBalancingSide");
                        if(PlayerManager.instance.handController.accroche) PlayerManager.instance.handController.handAnimator.SetTrigger("ChangeBalancingSide");
                        DoRotation(false);
                    });
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAvailable)
        {
            return;
        }
        if (other.gameObject.CompareTag("Hand") || other.gameObject.CompareTag("Body"))
        {
            playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.Accroche(this);
            playerController.gameObject.transform.eulerAngles = parent.transform.eulerAngles;
            isAvailable = false;
        }
    }
    
    public override IEnumerator OnLeave(Rigidbody2D rigidbody)
    {
        parent.transform.DOKill();
        parent.transform.DORotate(Vector3.zero,0.5f).SetEase(rotationEase);
        moving = false;
        rigidbody.linearVelocity = Vector2.zero;
        if (parent.transform.eulerAngles.z < 60)
        {
            rigidbody.AddForce(new (parent.transform.eulerAngles.z * strength, 0f));
        }
        else
        {
            rigidbody.AddForce(new (- (360 - parent.transform.eulerAngles.z) * strength, 0f));
        }
        yield return new WaitForSeconds(delayOnLeaving);
        isAvailable = true;
    }
}
