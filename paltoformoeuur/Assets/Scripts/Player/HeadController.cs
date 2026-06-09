using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HeadController : PlayerController
{
    [SerializeField] private Collider2D colliderCarre;
    [SerializeField] private Collider2D colliderRond;
    private int recallSpeed;
    private float initialAngularDamping;
    public bool isRecalling;
    private AudioSource audio;
    

    public override void Init(PlayerData data)
    {
        if (data is HeadData headData)
        {
            recallSpeed = headData.recallSpeed;
            gameObject.SetActive(false);
        }
    }

    protected override void Start()
    {
        base.Start();
        audio = GetComponent<AudioSource>();
        audio.clip = SoundManager.instance.rollingHead;
        audio.loop = true;
        audio.volume = SoundManager.instance.soundEffectVolume * SoundManager.instance.mainVolume;
        initialAngularDamping = elementRigidbody.angularDamping;
    }

    private void OnEnable()
    {
        colliderCarre.enabled = false;
        colliderRond.enabled = true;
    }

    protected void Update()
    {
        PlayerManager.instance.flameHead.transform.position = transform.position;
        
        if (Mathf.Abs(elementRigidbody.linearVelocity.x) < 0.2f)
        {
            elementRigidbody.linearVelocity = new Vector2(0, elementRigidbody.linearVelocity.y);
            colliderCarre.enabled = true;
            colliderRond.enabled = false;
            audio.Stop();
        }
        else
        {
            colliderCarre.enabled = false;
            colliderRond.enabled = true;
        }
    }
    
    public override void Recall()
    {
        if (isRecalling || PlayerManager.instance.headOnBody)
        {
            return;
        }

        audio.Stop();
        Debug.Log("stop");
        isRecalling = true;
        CameraManager.instance.ChangeTarget(PlayerPart.body);
        elementRigidbody.angularDamping = initialAngularDamping;
        base.Recall();
        transform.DOLocalMove(PlayerManager.instance.headAnchorPosition, Vector2.Distance(transform.position, player.transform.position) / recallSpeed)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
                {
                    Parallaxe.ChangeTarget(PlayerPart.body);
                    bodyScript.colliderWithHead.enabled = true;
                    bodyScript.colliderWithoutHead.enabled = false;
                    bodyScript.bodyAnimator.SetBool("IsHeadless",false);
                    DisableElement();
                    PlayerManager.instance.headOnBody = true;
                    PlayerManager.instance.flameHead.SetActive(false);
                    PlayerManager.instance.StartCoroutine(doLatter());
                    isRecalling = false;
                    gameObject.SetActive(false);
                    CameraManager.instance.CameraOnRecallHead();
                }
            );
        transform.DOLocalRotate(new Vector3(0, 0, 0), 1);
        
    }

    private IEnumerator doLatter()
    {
        yield return new WaitForSeconds(0.5f);
        bodyScript.canThrowHead = false;
    }

    public override void Die()
    {
        Recall();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            SoundManager.instance.PlaySound(SoundManager.instance.collidePart);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (colliderRond.enabled)
        {
            if (audio.isPlaying) return;
            audio.Play();
            Debug.Log("play");
        }
    }
}