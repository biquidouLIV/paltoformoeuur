using UnityEngine;

public class Animatique : MonoBehaviour
{
    public void EndAnimatique()
    {
        //UIManager.instance.LoadScene();
        gameObject.SetActive(false);
    }
}
