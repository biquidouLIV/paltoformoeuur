using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        UIManager.instance.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
