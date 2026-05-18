using System;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrochetBalance : Crochet
{
    [SerializeField] private float delayOnLeaving = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private float strength = 1;
    private bool isAvailable = true;
    private PlayerController playerController;
    private GameObject parent;
    public bool moving;
    private bool goingRight = true;

    private void Start()
    {
        parent = gameObject.transform.parent.gameObject;
    }

    private void FixedUpdate()
    {
       Move();
    }

    public void Move()
    {
        if (!moving)
        {
            if (Mathf.Abs(parent.transform.eulerAngles.z) < 60 && Mathf.Abs(parent.transform.eulerAngles.z) > 1)
            {
                parent.transform.Rotate(new Vector3(0, 0, -speed));
            }
            else if (Mathf.Abs(parent.transform.eulerAngles.z) > 60)
            {
                parent.transform.Rotate(new Vector3(0, 0, speed));
            }
        }
        else if (goingRight && (Mathf.Abs(parent.transform.eulerAngles.z) > 305 || Mathf.Abs(parent.transform.eulerAngles.z) < 50))
        {
            parent.transform.Rotate(new Vector3(0, 0, speed));
        }
        else if (Mathf.Abs(parent.transform.eulerAngles.z) > 310 || Mathf.Abs(parent.transform.eulerAngles.z) < 55)
        {
            goingRight = false;
            parent.transform.Rotate(new Vector3(0, 0, -speed));
        }
        else
        {
            goingRight = true;
            parent.transform.Rotate(new Vector3(0, 0, speed));
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
        }

        isAvailable = false;
    }
    
    public override IEnumerator Active(Rigidbody2D rigidbody)
    {
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
        moving = false;
    }
}
