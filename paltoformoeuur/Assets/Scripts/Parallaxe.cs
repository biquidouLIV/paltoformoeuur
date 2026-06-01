using UnityEngine;

public class Parallaxe : MonoBehaviour
{
    [SerializeField] private float speed;
    private static PlayerController target;
    private Vector3 lastPosition;
    
    private void Start()
    {
        target = PlayerManager.instance.bodyController;
        lastPosition = target.transform.position;
    }

    public static void ChangeTarget(PlayerPart playerPart)
    {
        if (playerPart == PlayerPart.body)
        {
            target = PlayerManager.instance.bodyController;
        }
        else
        {
            target = PlayerManager.instance.headController;
        }
    }
    
    private void FixedUpdate()
    {
        transform.Translate(new Vector2((lastPosition - target.transform.position).x * speed * Time.fixedDeltaTime, 0),Space.World);
        lastPosition = target.transform.position;
    }
}