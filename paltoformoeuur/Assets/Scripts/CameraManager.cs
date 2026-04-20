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

     private Vector3 direction = new Vector3(0,0,0);
     
     
     
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
         
         direction = (targetPart.transform.position - lastFramePosition);
         
         //quand on commence a descendre
         if((targetPart.transform.position - lastFramePosition).normalized == new Vector3(0, -1, 0) && direction == new Vector3(0, 1, 0))
         {
             //faire un truc ici
             Debug.Log("kaka");
         }
         
         //quand on atterit
         if (body.isGrounded)
         {
             direction.y = 0;
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
         
         direction.Normalize();




         
         Debug.Log(direction);
         cameraOffset = new Vector2(direction.x * horizontalDistance, direction.y * verticalDistance);
         target = targetPart.transform.position;
         
         if (targetPart == body)
         {
             testCamera.transform.DOMoveX(target.x + cameraOffset.x, horizontalSpeed)
                 .SetEase(ease);
             testCamera.transform.DOMoveY(target.y + cameraOffset.y, verticalSpeed)
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

