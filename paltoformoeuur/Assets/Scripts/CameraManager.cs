using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private float speed;

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject head;
    
    private GameObject targetBodyPart;
    
    private Vector2 direction;
    
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    private void Start()
    {
        SetOnBody();
    }
    
    public void SetOnBody()
    {
        targetBodyPart = body;
    }

    public void SetOnHand()
    {
        targetBodyPart = hand;
    }
    
    public void SetOnHead()
    {
        targetBodyPart = head;
    }
    
    private void Move()
    {
        if (Vector3.Distance(transform.position, targetBodyPart.transform.position) < 0.2)
        {
            return;
        }
        direction =  new Vector2(targetBodyPart.transform.position.x,targetBodyPart.transform.position.y) - new Vector2(transform.position.x,transform.position.y);
        direction *= speed;
        transform.Translate(direction);
    }

    private void FixedUpdate()
    {
        Move();
    }
}
