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
    

    private float horizontalDistance;
    private float verticalDistance;
    private float speed;
    private float bodyCameraFOV;
    private float headCameraFOV ;
    private float FOVTransitionDuration;
    
    private Camera testCamera; 
    private HeadController head; 
    private BodyController body;
    
        
     private float targetFOV;
     private PlayerController targetPart;
     
     private Vector3 lastFramePosition;
     private Vector3 cameraOffset;

     private Vector3 direction = new Vector3(0,0,0);
     
     private void Start()
     {
         body = PlayerManager.instance.bodyController;
         head = PlayerManager.instance.headController;
         
         horizontalDistance = data.horizontalDistance;
         verticalDistance = data.verticalDistance; 
         speed = data.speed;
         bodyCameraFOV = data.bodyCameraFOV;
         headCameraFOV = data.headCameraFOV;
         FOVTransitionDuration = data.FOVTransitionDuration;
     
         testCamera = gameObject.GetComponent<Camera>();
     
         targetPart = body;
         ChangeFOV(PlayerPart.body);
         
     }
     private void FixedUpdate()
     {
         
         //quand on commence a descendre
         if((targetPart.transform.position - lastFramePosition).normalized == new Vector3(0, -1, 0) && direction == new Vector3(0, 1, 0))
         {
             //faire un truc ici
             Debug.Log("kaka");
         }
         else
         {
             direction = (targetPart.transform.position - lastFramePosition).normalized;
         }

         
         //quand on atterit
         if (body.isGrounded)
         {
             direction = new Vector3(targetPart.transform.position.x - lastFramePosition.x, 0, 0).normalized;
         }
         else
         {
             direction = (targetPart.transform.position - lastFramePosition).normalized;
         }

         /*
         //quand on va dans un mur a gauche
         if ((targetPart.transform.position - lastFramePosition).normalized == new Vector3(-1, 0, 0) && direction == new Vector3(1, 0, 0))
         {
             direction = new Vector3(0, targetPart.transform.position.y - lastFramePosition.y, 0);
         }
         else
         {
             direction = (targetPart.transform.position - lastFramePosition).normalized;
         }
         
         //quand on va dans un mur a droite
         if ((targetPart.transform.position - lastFramePosition).normalized == new Vector3(1, 0, 0) && direction == new Vector3(-1, 0, 0))
         {
             direction = new Vector3(0, targetPart.transform.position.y - lastFramePosition.y, 0);
         }
         else
         {
             direction = (targetPart.transform.position - lastFramePosition).normalized;
         }
         */
         
         
         Debug.Log(direction);
         cameraOffset = new Vector3(direction.x * horizontalDistance, direction.y * verticalDistance,-10);
         
         if (targetPart == body)
         {
             testCamera.transform.DOMove(targetPart.transform.position + cameraOffset, speed).SetEase(Ease.OutCubic);
         }
         else
         {
             testCamera.transform.DOMove(new Vector3(targetPart.transform.position.x,targetPart.transform.position.y, -10), 0.5f*speed);
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

