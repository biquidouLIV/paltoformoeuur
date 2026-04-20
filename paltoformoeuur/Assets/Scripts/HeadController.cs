using DG.Tweening;
using UnityEngine;
public class HeadController : PlayerController
{
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
                    gameObject.SetActive(false);
                    PlayerManager.instance.PlayerInput.enabled = true;
                }
            );
        transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
    }
    
    public override void Die()
    {
        Recall();
    }
}