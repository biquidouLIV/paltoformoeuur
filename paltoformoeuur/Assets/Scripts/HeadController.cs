using DG.Tweening;
using UnityEngine;
public class HeadController : PlayerController
{
    [SerializeField] private int recallSpeed;
    private float initialAngularDamping;

    protected override void Start()
    {
        base.Start();
        initialAngularDamping = elementRigidbody.angularDamping;
    }
    
    public override void Recall()
    {
        elementRigidbody.angularDamping = initialAngularDamping;
        base.Recall();
        transform.DOLocalMove(PlayerManager.instance.headAnchorPosition, Vector2.Distance(transform.position, player.transform.position) / recallSpeed)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
                {
                    playerScript.colliderWithHead.enabled = true;
                    playerScript.colliderWithoutHead.enabled = false;
                    playerScript.bodyAnimator.SetBool("IsHeadless",false);
                    DisableElement();
                    PlayerManager.instance.headOnBody = true;
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