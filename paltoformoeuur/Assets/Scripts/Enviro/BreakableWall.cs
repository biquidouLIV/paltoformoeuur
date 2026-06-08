using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private float velocityToBreak;
    [SerializeField] private GameObject vfx;

    [SerializeField] private float dissolveTarget = 1.1f;
    [SerializeField] private float dissolveDuration = 2f;
    private Material material;
    
    
    private void Start()
    {
        material = GetComponentInChildren<SpriteRenderer>().material;
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Head"))
        {
            Rigidbody2D rigidbodyD = other.gameObject.GetComponent<Rigidbody2D>();
            GetComponent<BoxCollider2D>().enabled = false;
            DOTween.To(() => material.GetFloat("_DissolveAmount"), x => material.SetFloat("_DissolveAmount",x), dissolveTarget, dissolveDuration)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
            SoundManager.instance.PlaySound(SoundManager.instance.breakableWall);
            Instantiate(vfx,transform.position, Quaternion.identity);
            rigidbodyD.linearVelocity = new(rigidbodyD.linearVelocity.x / 2, rigidbodyD.linearVelocity.y / 2);
        }
    }


}
