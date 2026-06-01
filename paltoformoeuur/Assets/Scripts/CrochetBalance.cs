using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CrochetBalance : Crochet
{
    [SerializeField] private float delayOnLeaving = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private float strength = 1;
    [SerializeField] private Ease rotationEase;
    [SerializeField] private float timeForOneRotation;
    private bool isAvailable = true;
    private PlayerController playerController;
    private GameObject parent;
    public bool moving;
    private bool goingRight = true;

    private void Start()
    {
        parent = gameObject.transform.parent.gameObject;
    }

    public void StartRotation(bool goLeft)
    {
        moving = true;
        DoRotation(goLeft);
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
                        PlayerManager.instance.bodyController.bodyAnimator.SetTrigger("ChangeBalancingSide");
                        DoRotation(!left);
                    });
            }
            else
            {
                parent.transform.DORotate(new Vector3(0, 0, 310), timeForOneRotation)
                    .SetEase(rotationEase).OnComplete(() =>
                    {
                        PlayerManager.instance.bodyController.bodyAnimator.SetTrigger("ChangeBalancingSide");
                        DoRotation(!left);
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
            goingRight = playerController.gameObject.transform.position.x < transform.position.x;
            playerController.gameObject.transform.eulerAngles = parent.transform.eulerAngles;
            isAvailable = false;
        }
    }
    
    public override IEnumerator Active(Rigidbody2D rigidbody)
    {
        parent.transform.DOKill();
        parent.transform.DORotate(Vector3.zero,2).SetEase(rotationEase);
        moving = false;
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
