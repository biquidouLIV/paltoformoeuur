using System;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int length = 20;
    [SerializeField] private float sensitivity = 0.1f;
    private Vector3 oldCurveLastPoint;

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
        
        
       
        if(Vector3.Distance(oldCurveLastPoint,pts[length-1]) < sensitivity)
        {
            return;
        }

        oldCurveLastPoint = pts[pts.Length - 1];
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
