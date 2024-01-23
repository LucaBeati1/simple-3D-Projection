using Godot;
using System;
using System.Linq;

public partial class Object3D : RefCounted
{
    public Polygon[] polygons;

    public Object3D()
    {
    }

    public void addPolygons(params Polygon[] polygons)
    {
        this.polygons = polygons;
    }
}
