using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private float velocityToBreak;
    [SerializeField] private GameObject vfx;

    [SerializeField] private float dissolveTarget = 1.1f;
    [SerializeField] private float dissolveDuration = 2f;
    private Material material;
    private bool isDestroying;
    
    
    private void Start()
    {
        material = GetComponentInChildren<SpriteRenderer>().material;
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Head") && !isDestroying)
        {
            isDestroying = true;
            Rigidbody2D rigidbodyD = other.gameObject.GetComponent<Rigidbody2D>();
            GetComponent<BoxCollider2D>().enabled = false;
            DOTween.To(() => material.GetFloat("_DissolveAmount"), x => material.SetFloat("_DissolveAmount",x), dissolveTarget, dissolveDuration)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
            SoundManager.instance.PlaySound(SoundManager.instance.breakableWall);
            Instantiate(vfx,GetComponentInChildren<SpriteRenderer>().bounds.center, Quaternion.identity);
            rigidbodyD.linearVelocity = new(rigidbodyD.linearVelocity.x / 2, rigidbodyD.linearVelocity.y / 2);
        }
    }


}
