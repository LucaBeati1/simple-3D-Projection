using Godot;
using System;

public partial class Camera : RefCounted
{
    public Vector3 pointA = new Vector3();
    public Vector3 pointB = new Vector3();
    public Vector3 pointC = new Vector3();
    public Vector3 pointObserver = new Vector3();
    public Vector3 midPoint = new Vector3();
    public Vector3 vectorAB = new Vector3();
    public Vector3 vectorAC = new Vector3();
    public Vector3 vectorN = new Vector3();
    public float distance;
    public float multiplicant;

    public Camera(Vector3 pointA, Vector3 pointB, Vector3 pointC, float distance)
    {
        this.pointA = pointA;
        this.pointB = pointB;
        this.pointC = pointC;
        this.distance = distance;
        vectorAB = pointB - pointA;
        vectorAC = pointC - pointA;
        vectorN = vectorAC.Cross(vectorAB);
        midPoint = pointA + vectorAB / 2 + vectorAC / 2;
        multiplicant = distance / vectorN.Length();
        pointObserver = midPoint + vectorN * multiplicant * -1;
    }
}
