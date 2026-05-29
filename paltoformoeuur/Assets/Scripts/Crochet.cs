using System.Collections;
using UnityEngine;

// autant en faite une interface dans ton cas, tu n'as absolument rien du tout dans la classe et ca a pas l'air d'avoir vocation à changer
public abstract class Crochet : MonoBehaviour
{
    public abstract IEnumerator Active(Rigidbody2D rigidbody);
}
