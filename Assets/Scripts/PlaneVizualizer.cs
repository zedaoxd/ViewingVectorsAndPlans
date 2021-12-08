using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Operation
{
    None,
    IsInFront,
    ProjectPoint,
    ProjectVector,
}

public class PlaneVizualizer : MonoBehaviour
{
    [SerializeField] private Vector3 p1;
    [SerializeField] private Vector3 p2;
    [SerializeField] private Vector3 p3;
    [SerializeField] private Vector3 point;
    [SerializeField] private Operation operation;

    private const float VecThickess = 5;
    private const float SphereRadius = 0.1f;

    private void OnDrawGizmos()
    {
        var plane = new MyPlane(p1, p2, p3);
        DrawBase();

        if (plane.IsInFront(point))
        {
            DrawPlane(plane);
            DrawPlaneOperation(plane);
        }
        else
        {
            DrawPlaneOperation(plane);
            DrawPlane(plane);
        }
    }

    private void DrawPlaneOperation(in MyPlane plane)
    {
        switch (operation)
        {
            case Operation.IsInFront:
                DrawIsInFront(plane);
                break;
            case Operation.ProjectVector:
                DrawProjectVector(plane);
                break;
            case Operation.ProjectPoint:
                DrawProjectPoint(plane);
                break;
            case Operation.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DrawIsInFront(in MyPlane plane)
    {
        Gizmos.color = plane.IsInFront(point) ? Color.red : Color.green;
        Gizmos.DrawSphere(point, SphereRadius);
    }

    private void DrawProjectVector(in MyPlane plane)
    {
        Gizmos.color = Color.cyan;
        GizmosUtils.DrawVector(p1, point, VecThickess);
        
        Gizmos.color = Color.blue;
        var projected = plane.ProjectVector(point);
        GizmosUtils.DrawVector(p1, projected, VecThickess);

        var projectedNormal = projected - point;
        Gizmos.color = Color.cyan;
        GizmosUtils.DrawVector(p1 + point, projectedNormal, SphereRadius);
    }

    private void DrawProjectPoint(in MyPlane plane)
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(point, SphereRadius);
        
        Gizmos.color = Color.blue;
        var projected = plane.ProjectPoint(point);
        GizmosUtils.DrawVectorAtOrigin(projected, VecThickess * 0.5f);
        Gizmos.DrawSphere(projected, SphereRadius);

        var projectedNormal = projected - point;
        Gizmos.color = Color.cyan;
        GizmosUtils.DrawVector(point, projectedNormal, 0.1f);
    }

    private void DrawPlane(in MyPlane plane)
    {
        Gizmos.color = Color.white;
        var distanceVec = plane.Distance * plane.Normal;
        GizmosUtils.DrawVectorAtOrigin(distanceVec, 0.1f);

        var size = Mathf.Max((p2 - p1).magnitude, (p3 - p1).magnitude);
        GizmosUtils.DrawPlane(plane.Normal, plane.Point, Vector2.one * size * 2);

        Gizmos.color = Color.magenta;
        GizmosUtils.DrawVector(p1, plane.Normal, VecThickess);
        
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(p1, SphereRadius);
        Gizmos.DrawSphere(p2, SphereRadius);
        Gizmos.DrawSphere(p3, SphereRadius);
    }

    private void DrawBase()
    {
        Gizmos.color = Color.red;
        GizmosUtils.DrawVectorAtOrigin(Vector3.right, VecThickess);
        Gizmos.color = Color.green;
        GizmosUtils.DrawVectorAtOrigin(Vector3.up, VecThickess);
        Gizmos.color = Color.blue;
        GizmosUtils.DrawVectorAtOrigin(Vector3.forward, VecThickess);
    }
}
