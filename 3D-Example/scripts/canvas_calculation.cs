using Godot;
using System;

public partial class canvas_calculation : Node
{
    public Camera camera = new Camera(new Vector3(0,0,0),new Vector3(12,0,0),new Vector3(0,0,9),10);
    public Object3D cube = new Object3D();
    public Object3D cube2 = new Object3D();
    public Vector3 lightDirection = new Vector3(4, 0, 5);
    Vector3 pointA = new Vector3(4.5f,4,0);
    Vector3 pointB = new Vector3(7.5f,4,0);
    Vector3 pointC = new Vector3(4.5f,4,3);
    Vector3 pointD = new Vector3(7.5f,4,3);
    Vector3 pointE = new Vector3(4.5f,7,0);
    Vector3 pointF = new Vector3(7.5f,7,0);
    Vector3 pointG = new Vector3(4.5f,7,3);
    Vector3 pointH = new Vector3(7.5f,7,3);

    public override void _Ready()
    {
        cube.addPolygons
        (
            new Polygon(new Vector3(4.5f,4,0), new Vector3(4.5f,4,3), new Vector3(7.5f,4,3)),
            new Polygon(new Vector3(4.5f,4,0), new Vector3(7.5f,4,3), new Vector3(7.5f,4,0)),
            new Polygon(new Vector3(7.5f,4,0), new Vector3(7.5f,4,3), new Vector3(7.5f,7,3)),
            new Polygon(new Vector3(7.5f,4,0), new Vector3(7.5f,7,3), new Vector3(7.5f,7,0)),
            new Polygon(new Vector3(7.5f,7,0), new Vector3(7.5f,7,3), new Vector3(4.5f,7,3)),
            new Polygon(new Vector3(7.5f,7,0), new Vector3(4.5f,7,3), new Vector3(4.5f,7,0)),
            new Polygon(new Vector3(4.5f,7,0), new Vector3(4.5f,7,3), new Vector3(4.5f,4,3)),
            new Polygon(new Vector3(4.5f,7,0), new Vector3(4.5f,4,3), new Vector3(4.5f,4,0)),
            new Polygon(new Vector3(4.5f,4,3), new Vector3(4.5f,7,3), new Vector3(7.5f,7,3)),
            new Polygon(new Vector3(4.5f,4,3), new Vector3(7.5f,7,3), new Vector3(7.5f,4,3)),
            new Polygon(new Vector3(7.5f,4,0), new Vector3(7.5f,7,0), new Vector3(4.5f,7,0)),
            new Polygon(new Vector3(7.5f,4,0), new Vector3(4.5f,7,0), new Vector3(4.5f,4,0))
        );
        cube2.addPolygons
        (
            new Polygon(new Vector3(4.5f,4,0), new Vector3(4.5f,4,3), new Vector3(7.5f,4,3)),
            new Polygon(new Vector3(4.5f,4,0), new Vector3(7.5f,4,3), new Vector3(7.5f,4,0)),
            new Polygon(new Vector3(7.5f,4,0), new Vector3(7.5f,4,3), new Vector3(7.5f,7,3)),
            new Polygon(new Vector3(7.5f,4,0), new Vector3(7.5f,7,3), new Vector3(7.5f,7,0)),
            new Polygon(new Vector3(7.5f,7,0), new Vector3(7.5f,7,3), new Vector3(4.5f,7,3)),
            new Polygon(new Vector3(7.5f,7,0), new Vector3(4.5f,7,3), new Vector3(4.5f,7,0)),
            new Polygon(new Vector3(4.5f,7,0), new Vector3(4.5f,7,3), new Vector3(4.5f,4,3)),
            new Polygon(new Vector3(4.5f,7,0), new Vector3(4.5f,4,3), new Vector3(4.5f,4,0)),
            new Polygon(new Vector3(4.5f,4,3), new Vector3(4.5f,7,3), new Vector3(7.5f,7,3)),
            new Polygon(new Vector3(4.5f,4,3), new Vector3(7.5f,7,3), new Vector3(7.5f,4,3)),
            new Polygon(new Vector3(7.5f,4,0), new Vector3(7.5f,7,0), new Vector3(4.5f,7,0)),
            new Polygon(new Vector3(7.5f,4,0), new Vector3(4.5f,7,0), new Vector3(4.5f,4,0))
        );
    }

    public override void _PhysicsProcess(double delta)
    {
        rotateCubeZClockwise(cube, 3 * (float)delta);
    }

    public Polygon canvasPolygon(Polygon polygon)
    {
        Polygon result = new Polygon(polygon.pointA, polygon.pointB, polygon.pointC);
        result.pointA = canvasPoint(polygon.pointA);
        result.pointB = canvasPoint(polygon.pointB);
        result.pointC = canvasPoint(polygon.pointC);

        return result;
    }

    public Vector3 canvasPoint(Vector3 pointX)
    {
        Vector3 vectorX = camera.pointObserver - pointX;
        float cosA = (camera.vectorN.X * vectorX.X + camera.vectorN.Y * vectorX.Y + camera.vectorN.Z * vectorX.Z) / (camera.vectorN.Length() * vectorX.Length());
        float hypotenuse = camera.distance / cosA;
        float factor = hypotenuse / vectorX.Length();
        Vector3 result = camera.pointObserver + vectorX * factor;

        return result;
    }

    public Vector2 pointTo2D(Vector3 point)
    {
        float u = 1;
        float v = 1;

        if(camera.vectorAB.X * camera.vectorAC.Y - camera.vectorAB.Y * camera.vectorAC.X != 0 && camera.vectorAC.X != 0)
        {
            u = camera.pointA.Y * camera.vectorAC.X + (point.X - camera.pointA.X) * camera.vectorAC.Y - point.Y * camera.vectorAC.X /
            (camera.vectorAB.X * camera.vectorAC.Y - camera.vectorAB.Y * camera.vectorAC.X);
            v = point.X - camera.pointA.X - u * camera.vectorAB.X / camera.vectorAC.X;
        }

        else if(camera.vectorAB.Z * camera.vectorAC.X - camera.vectorAB.X * camera.vectorAC.Z != 0 && camera.vectorAC.Z != 0)
        {
            u = camera.pointA.X * camera.vectorAC.Z + (point.Z - camera.pointA.Z) * camera.vectorAC.X - point.X * camera.vectorAC.Z /
            (camera.vectorAB.Z * camera.vectorAC.X - camera.vectorAB.X * camera.vectorAC.Z);
            v = (point.Z - camera.pointA.Z - u * camera.vectorAB.Z) / camera.vectorAC.Z;
        }

        else if(camera.vectorAB.Y * camera.vectorAC.Z - camera.vectorAB.Z * camera.vectorAC.Y != 0 && camera.vectorAC.Y != 0)
        {
            u = camera.pointA.Y * camera.vectorAC.Z + (point.Z - camera.pointA.Z) * camera.vectorAC.Y - point.Y * camera.vectorAC.Z /
            (camera.vectorAB.Z * camera.vectorAC.Y - camera.vectorAB.Y * camera.vectorAC.Z);
            v = point.Z - camera.pointA.Z - u * camera.vectorAB.Z / camera.vectorAC.Z;
        }

        Vector2 result;
        result.X = camera.vectorAB.Length() * u * 100;
        result.Y = camera.vectorAC.Length() * v * -100 + 900;

        return result;
    }

    public float culling(Polygon polygon)
    {
        Vector3 vectorAO = camera.pointObserver - polygon.pointA;
        Vector3 vectorAB = polygon.pointB - polygon.pointA;
        Vector3 vectorAC = polygon.pointC - polygon.pointA;
        Vector3 vectorN = vectorAB.Cross(vectorAC);

        float cosA = (vectorN.X * vectorAO.X + vectorN.Y * vectorAO.Y + vectorN.Z * vectorAO.Z) / (vectorN.Length() * vectorAO.Length());

        return cosA;
    }

    public void rotateCubeZCounter(Object3D cube, float degree)
    {
        Vector3 vectorAH = pointH - pointA;
        Vector3 pointM = pointA + vectorAH / 2;

        foreach(Polygon polygon in cube.polygons)
        {
            polygon.pointA = pointM + rotatePointZCounter(pointM, polygon.pointA, degree);;
            polygon.pointB = pointM + rotatePointZCounter(pointM, polygon.pointB, degree);;
            polygon.pointC = pointM + rotatePointZCounter(pointM, polygon.pointC, degree);;
        }
    }

    public void rotateCubeZClockwise(Object3D cube, float degree)
    {
        Vector3 vectorAH = pointH - pointA;
        Vector3 pointM = pointA + vectorAH / 2;

        foreach(Polygon polygon in cube.polygons)
        {
            polygon.pointA = pointM + rotatePointZClockwise(pointM, polygon.pointA, degree);;
            polygon.pointB = pointM + rotatePointZClockwise(pointM, polygon.pointB, degree);;
            polygon.pointC = pointM + rotatePointZClockwise(pointM, polygon.pointC, degree);;
        }
    }

    public Vector3 rotatePointZCounter(Vector3 pointM, Vector3 pointX, float degree)
    {
        Vector3 vectorMX = pointX - pointM;
        Vector3 result;
        float[,] rotationMatrix = new float[3,3];
        rotationMatrix[0,0] = (float)Math.Cos(degree); rotationMatrix[0,1] = (float)-Math.Sin(degree); rotationMatrix[0,2] = 0;
        rotationMatrix[1,0] = (float)Math.Sin(degree); rotationMatrix[1,1] = (float)Math.Cos(degree); rotationMatrix[1,2] = 0;
        rotationMatrix[2,0] = 0; rotationMatrix[2,1] = 0; rotationMatrix[2,2] = 1;

        result.X = rotationMatrix[0,0] * vectorMX.X + rotationMatrix[0,1] * vectorMX.Y + rotationMatrix[0,2] * vectorMX.Z;
        result.Y = rotationMatrix[1,0] * vectorMX.X + rotationMatrix[1,1] * vectorMX.Y + rotationMatrix[1,2] * vectorMX.Z;
        result.Z = rotationMatrix[2,0] * vectorMX.X + rotationMatrix[2,1] * vectorMX.Y + rotationMatrix[2,2] * vectorMX.Z;
        
        return result;    
    }

    public Vector3 rotatePointZClockwise(Vector3 pointM, Vector3 pointX, float degree)
    {
        Vector3 vectorMX = pointX - pointM;
        Vector3 result;
        float[,] rotationMatrix = new float[3,3];
        rotationMatrix[0,0] = (float)Math.Cos(degree); rotationMatrix[0,1] = (float)Math.Sin(degree); rotationMatrix[0,2] = 0;
        rotationMatrix[1,0] = (float)-Math.Sin(degree); rotationMatrix[1,1] = (float)Math.Cos(degree); rotationMatrix[1,2] = 0;
        rotationMatrix[2,0] = 0; rotationMatrix[2,1] = 0; rotationMatrix[2,2] = 1;

        result.X = rotationMatrix[0,0] * vectorMX.X + rotationMatrix[0,1] * vectorMX.Y + rotationMatrix[0,2] * vectorMX.Z;
        result.Y = rotationMatrix[1,0] * vectorMX.X + rotationMatrix[1,1] * vectorMX.Y + rotationMatrix[1,2] * vectorMX.Z;
        result.Z = rotationMatrix[2,0] * vectorMX.X + rotationMatrix[2,1] * vectorMX.Y + rotationMatrix[2,2] * vectorMX.Z;
        
        return result;    
    }

    public Color lightColor(Polygon polygon, Color color)
	{
        Vector3 vectorAB = polygon.pointB - polygon.pointA;
        Vector3 vectorAC = polygon.pointC - polygon.pointA;
        Vector3 vectorN = vectorAB.Cross(vectorAC);
        float cosA = (vectorN.X * lightDirection.X + vectorN.Y * lightDirection.Y + vectorN.Z * lightDirection.Z) / (vectorN.Length() * lightDirection.Length());

        color = color.Darkened(cosA);
		
		return color;
	}

    public float getDistanceFromCamera(Vector3 point, Vector3 ObserverPoint)
    {
        Vector3 distance = point - ObserverPoint;
        float result = distance.Length();

        return result;
    }
}
