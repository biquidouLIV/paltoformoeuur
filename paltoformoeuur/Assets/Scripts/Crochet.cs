using System.Collections;
using UnityEngine;

public abstract class Crochet : MonoBehaviour
{
    public abstract IEnumerator Active(Rigidbody2D rigidbody);
}
