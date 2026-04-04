using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject fallingAnchor;
    [SerializeField] private float speed = 2;
    public bool falling;
    
    private void Start()
    {
        fallingAnchor.SetActive(false);
    }
    public void Update()
    {
        if (falling && transform.position.y > fallingAnchor.transform.position.y)
        {
            transform.Translate(new Vector2(0, -1 * speed * Time.deltaTime),Space.World);
        }
    }
}
