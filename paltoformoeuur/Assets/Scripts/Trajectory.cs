using System;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int length = 20;

    private void Start()
    {
        lineRenderer.positionCount = length;
    }

    public void TrajectoryCalcul(Vector2 startPosition, Vector2 velocity)
    {
        ShowTrajectory();
        Vector3[] pts = new Vector3[length];

        for (int i = 0; i < length; i++)
        {
            float t = i * 0.1f;
            Vector2 pos = startPosition + velocity * t + (Physics2D.gravity * t * t / 2);
            pts[i] = new Vector3(pos.x, pos.y, 0);
        }
        lineRenderer.SetPositions(pts);
    }

    public void HideTrajectory()
    {
        lineRenderer.enabled = false;
    }

    public void ShowTrajectory()
    {
        lineRenderer.enabled = true;
    }
}
