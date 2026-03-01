using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private float speed;

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject head;

    [SerializeField] private float bodyCameraFOV = 5f;
    [SerializeField] private float handCameraFOV = 1.5f;
    [SerializeField] private float headCameraFOV = 8f;
    [SerializeField] private float FOVTransitionSpeed = 0.2f;

    private float targetFOV;

    private Camera camera;
    private GameObject targetBodyPart;
    
    private Vector2 direction;
    
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    private void Start()
    {
        camera = GetComponent<Camera>();
        SetOnBody();
    }

    private void Update()
    {
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetFOV,FOVTransitionSpeed);
    }

    public void SetOnBody()
    {
        targetBodyPart = body;
        targetFOV = bodyCameraFOV;
    }

    public void SetOnHand()
    {
        targetBodyPart = hand;
        targetFOV = handCameraFOV;
    }
    
    public void SetOnHead()
    {
        targetBodyPart = head;
        targetFOV = headCameraFOV;
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
