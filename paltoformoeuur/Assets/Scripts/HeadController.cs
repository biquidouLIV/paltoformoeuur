using System;
using DG.Tweening;
using UnityEngine;
public class HeadController : PlayerController
{
    [SerializeField] private Collider2D colliderCarre;
    [SerializeField] private Collider2D colliderRond;
    private int recallSpeed;
    private float initialAngularDamping;


    public override void Init(PlayerData data)
    {
        if (data is HeadData headData)
        {
            recallSpeed = headData.recallSpeed;
        }
    }

    protected override void Start()
    {
        base.Start();
        initialAngularDamping = elementRigidbody.angularDamping;
    }

    private void OnEnable()
    {
        colliderCarre.enabled = false;
        colliderRond.enabled = true;
    }

    protected void Update()
    {
        if (Mathf.Abs(elementRigidbody.linearVelocity.x) < 0.2f)
        {
            elementRigidbody.linearVelocity = new Vector2(0, elementRigidbody.linearVelocity.y);
            colliderCarre.enabled = true;
            colliderRond.enabled = false;
        }
        else
        {
            colliderCarre.enabled = false;
            colliderRond.enabled = true;
        }
    }
    
    public override void Recall()
    {
        CameraManager.instance.ChangeFOV(PlayerPart.body);
        elementRigidbody.angularDamping = initialAngularDamping;
        base.Recall();
        transform.DOLocalMove(PlayerManager.instance.headAnchorPosition, Vector2.Distance(transform.position, player.transform.position) / recallSpeed)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
                {
                    bodyScript.colliderWithHead.enabled = true;
                    bodyScript.colliderWithoutHead.enabled = false;
                    bodyScript.bodyAnimator.SetBool("IsHeadless",false);
                    DisableElement();
                    PlayerManager.instance.headOnBody = true;
                    PlayerManager.instance.PlayerInput.enabled = true;
                    gameObject.SetActive(false);
                }
            );
        transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
    }
    
    public override void Die()
    {
        Recall();
    }
}