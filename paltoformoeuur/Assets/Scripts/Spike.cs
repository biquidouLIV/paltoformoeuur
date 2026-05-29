using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // si tu veux éviter le double get Compo couteux tu peux faire:
        // var playerController =other.GetComponent<PlayerController>() ;
        // if( playerController != null
        // c'est pas grand chose, mais on badine pas sur les petites économies
        
        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().Die();
        }
    }
}
