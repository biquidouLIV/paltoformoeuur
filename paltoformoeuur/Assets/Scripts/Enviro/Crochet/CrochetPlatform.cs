using System;
using System.Collections;
using UnityEngine;

public class CrochetPlatform : Crochet
{
    [SerializeField] private float delayOnLeaving = 1;
    public FallingPlatform fallingPlatform;
    private bool isAvailable = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAvailable)
        {
            return;
        }
        if (other.gameObject.CompareTag("Hand"))
        {
            SoundManager.instance.PlaySound(SoundManager.instance.crochet);
            other.gameObject.GetComponent<PlayerController>().Accroche(this, fallingPlatform);
            isAvailable = false;
        }
    }
    
    public override IEnumerator OnLeave(Rigidbody2D rigidbody)
    {
        fallingPlatform.falling = false;
        isAvailable = false;
        yield return new WaitForSeconds(delayOnLeaving);
        isAvailable = true;
    }
}
