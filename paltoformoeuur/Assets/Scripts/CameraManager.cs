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

    [SerializeField] private Vector2 defaultOffset;
    
    
    [SerializeField] private CameraData data;
    [SerializeField] private Ease ease;

    private float horizontalDistance;
    private float verticalDistance;
    private float horizontalSpeed;
    private float verticalSpeed;
    private float bodyCameraFOV;
    private float headCameraFOV ;
    private float FOVTransitionDuration;
    
    private Camera testCamera; 
    private HeadController head; 
    private BodyController body;
    
        
     private float targetFOV;
     private PlayerController targetPart;
     
     private Vector3 lastFramePosition;
     private Vector2 cameraOffset;
     private Vector3  target;
     private Vector3 destination;

     private float directionX;
     private float directionY;

     [SerializeField] private float multiplicationDeQuandTuVasEnBas = 1.5f;
     private float multiplicationDeQuandTuVasEnBasMultiplicateur;
     [SerializeField] private Vector2 YMaxDistance = new Vector2(-3,3);
     [SerializeField] private Vector2 XMaxDistance = new Vector2(-3,3);
     
     private void Start()
     {
         body = PlayerManager.instance.bodyController;
         head = PlayerManager.instance.headController;
         
         horizontalDistance = data.horizontalDistance;
         verticalDistance = data.verticalDistance; 
         horizontalSpeed = data.horizontalSpeed;
         verticalSpeed = data.verticalSpeed;
         bodyCameraFOV = data.bodyCameraFOV;
         headCameraFOV = data.headCameraFOV;
         FOVTransitionDuration = data.FOVTransitionDuration;
     
         testCamera = gameObject.GetComponent<Camera>();
     
         targetPart = body;
         target = targetPart.transform.position;
         ChangeFOV(PlayerPart.body);
         
     }
     private void FixedUpdate()
     {
         
        SetOffset();
        Move();
        
         
     }

     private void SetOffset()
     {
         directionX = targetPart.transform.position.x - lastFramePosition.x;
         directionY = targetPart.transform.position.y - lastFramePosition.y;
         
         
         //quand on commence a descendre
         if((targetPart.transform.position - lastFramePosition).normalized == new Vector3(0, -1, 0) && directionY == 1)
         {
             //jsp
         }
         
         //quand on atterit
         if (body.isGrounded)
         {
             directionY = 0;
         }
         
         //quand on va dans un mur a gauche
         if ((targetPart.transform.position - lastFramePosition).normalized == new Vector3(-1, 0, 0) && PlayerManager.instance.bodyController.moveInput.x >= 0)
         {
             directionX = 0;
         }
         
         //quand on va dans un mur a droite
         if ((targetPart.transform.position - lastFramePosition).normalized == new Vector3(1, 0, 0) && PlayerManager.instance.bodyController.moveInput.x <= 0)
         {
             directionX = 0;
         }

         if (directionY < 0)
         {
             multiplicationDeQuandTuVasEnBasMultiplicateur = multiplicationDeQuandTuVasEnBas;
         }
         else
         {
             multiplicationDeQuandTuVasEnBasMultiplicateur = 1;
         }
         
         cameraOffset = new Vector2(directionX * horizontalDistance, directionY * verticalDistance * multiplicationDeQuandTuVasEnBasMultiplicateur);
     }

     private void Move()
     {
         target = targetPart.transform.position;

         destination = (Vector2)target + defaultOffset + cameraOffset;
         
         
         destination.x = Mathf.Clamp(destination.x, XMaxDistance.x, XMaxDistance.y);
         destination.y = Mathf.Clamp(destination.y, YMaxDistance.x, YMaxDistance.y);
         
         if (targetPart == body)
         {
             
             testCamera.transform.DOMoveX(target.x + defaultOffset.x + cameraOffset.x, horizontalSpeed)
                 .SetEase(ease);
             testCamera.transform.DOMoveY(target.y + defaultOffset.y + cameraOffset.y, verticalSpeed)
                 .SetEase(ease);
         }
         else
         {
             testCamera.transform.DOMoveX(target.x, horizontalSpeed)
                 .SetEase(ease);
             testCamera.transform.DOMoveY(target.y, verticalSpeed)
                 .SetEase(ease);
         }
         
         lastFramePosition = targetPart.transform.position;
     }
     
    public void ChangeFOV(PlayerPart part)
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
        testCamera.DOOrthoSize(targetFOV, FOVTransitionDuration);
    }
}

