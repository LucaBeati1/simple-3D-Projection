using Godot;
using System;

public partial class Polygon : RefCounted
{
    public Vector3 pointA;
    public Vector3 pointB;
    public Vector3 pointC;

    public Polygon(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        this.pointA = pointA;
        this.pointB = pointB;
        this.pointC = pointC;
    }
}
