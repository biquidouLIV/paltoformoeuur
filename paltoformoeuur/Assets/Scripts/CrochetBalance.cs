using System.Collections;
using UnityEngine;

public class CrochetBalance : Crochet
{
    [SerializeField] private float delayOnLeaving = 1;
    private bool isAvailable = true;
    private PlayerController playerController;
    private GameObject parent;
    public bool moving;
    private bool goingRight = true;

    private void Start()
    {
        parent = gameObject.transform.parent.gameObject;
    }

    private void Update()
    {
        if (moving) Move();
    }

    public void Move()
    {
        if (goingRight && (Mathf.Abs(parent.transform.eulerAngles.z) > 305 || Mathf.Abs(parent.transform.eulerAngles.z) < 50))
        {
            parent.transform.Rotate(new Vector3(0, 0, 1));
        }
        else if (Mathf.Abs(parent.transform.eulerAngles.z) > 310 || Mathf.Abs(parent.transform.eulerAngles.z) < 55)
        {
            goingRight = false;
            parent.transform.Rotate(new Vector3(0, 0, -1));
        }
        else
        {
            goingRight = true;
            parent.transform.Rotate(new Vector3(0, 0, 1));
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
        }

        isAvailable = false;
    }
    
    public override IEnumerator Active()
    {
        yield return new WaitForSeconds(delayOnLeaving);
        isAvailable = true;
    }
}
