using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private Vector2 defaultOffset;
    
    
    [SerializeField] private CameraData data;
    [SerializeField] private Ease horizontalEase;
    [SerializeField] private Ease verticalEase;
    
    
    private Camera testCamera; 
    private HeadController head; 
    private BodyController body;
    private PlayerController targetPart;
    
    
    private Vector2 cameraOffset;
    
    private float horizontalDistance;
    private float horizontalSpeed;
    
    private float verticalDistance;
    private float verticalSpeed;
    private float minVelocity;
    private float minDistanceWithGound;
    
    private float bodyCameraFOV;
    private float headCameraFOV ;
    private float FOVTransitionDuration;
    private float targetFOV;

    private Vector3 lastFramePosition;
    private Vector3 targetPosition;
    private Vector3 destination;
    private Vector2 direction;

    
     private void Start()
     {
         
        body = PlayerManager.instance.bodyController;
        head = PlayerManager.instance.headController;

        horizontalDistance = data.horizontalDistance;
        horizontalSpeed = data.horizontalSpeed;

        verticalDistance = data.verticalDistance;
        verticalSpeed = data.verticalSpeed;
        minVelocity = data.minVelocity;
        minDistanceWithGound = data.minDistanceWithGround;

        bodyCameraFOV = data.bodyCameraFOV;
        headCameraFOV = data.headCameraFOV;
        FOVTransitionDuration = data.FOVTransitionDuration;

        defaultOffset = data.defaultOffset;


        testCamera = gameObject.GetComponent<Camera>();
        targetPart = body;
        targetPosition = targetPart.transform.position;
        ChangeTarget(PlayerPart.body);
         
     }
     private void FixedUpdate()
     {
        SetOffset();
        Move();
     }

     private void SetOffset()
     {
         direction.x = targetPart.transform.position.x - lastFramePosition.x;
         direction.y = targetPart.transform.position.y - lastFramePosition.y;
         
         //vertical
         if ((direction.y < -minVelocity) && (body.distanceWithGround >= minDistanceWithGound))
         {
            cameraOffset.y = -verticalDistance;
         }
         else
         {
             cameraOffset.y = 0;
         }
         
         
         //quand on va dans un mur a gauche
         if ((targetPart.transform.position - lastFramePosition).normalized == new Vector3(-1, 0, 0) && PlayerManager.instance.bodyController.moveInput.x >= 0)
         {
             direction.x = 0;
         }
         
         //quand on va dans un mur a droite
         if ((targetPart.transform.position - lastFramePosition).normalized == new Vector3(1, 0, 0) && PlayerManager.instance.bodyController.moveInput.x <= 0)
         {
             direction.x = 0;
         }
         
         
         
         cameraOffset.x = direction.x * horizontalDistance;
     }

     private void Move()
     {
         targetPosition = targetPart.transform.position;

         destination = (Vector2)targetPosition + cameraOffset + defaultOffset;

         
         if (targetPart == body)
         {
             
             testCamera.transform.DOMoveX(destination.x, horizontalSpeed)
                 .SetEase(horizontalEase);
             testCamera.transform.DOMoveY(destination.y, verticalSpeed)
                 .SetEase(verticalEase);
         }
         else
         {
             testCamera.transform.DOMoveX(targetPosition.x, horizontalSpeed)
                 .SetEase(horizontalEase);
             testCamera.transform.DOMoveY(targetPosition.y, verticalSpeed)
                 .SetEase(verticalEase);
         }
         
         
         lastFramePosition = targetPart.transform.position;
     }
     
    public void ChangeTarget(PlayerPart part)
    {
        switch (part)
        {
            case PlayerPart.body:
                targetFOV = bodyCameraFOV;
                targetPart = body;
                break;
            case PlayerPart.head:
                targetFOV = headCameraFOV;
                targetPart = head;
                break;
        }
        lastFramePosition = targetPart.transform.position;
        
        testCamera.DOOrthoSize(targetFOV, FOVTransitionDuration);
    }
}

