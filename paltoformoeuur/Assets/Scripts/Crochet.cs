using System.Collections;
using UnityEngine;

public abstract class Crochet : MonoBehaviour
{
    public abstract IEnumerator OnLeave(Rigidbody2D rigidbody);
}
