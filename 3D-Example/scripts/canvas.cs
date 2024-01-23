using Godot;
using System;

public partial class canvas : Node2D
{
	public canvas_calculation canvasCalculation;
	public BufferedImage bufferedImage;
	public Sprite2D screen;

	public override void _Ready()
	{
		canvasCalculation = GetNode<canvas_calculation>("canvas_calculation");
		screen = GetNode<Sprite2D>("Screen");
		bufferedImage = new BufferedImage(1200, 900, new Color(0.9f,0.9f,0.95f,1));
		fillRasterizedObject3D(canvasCalculation.cube, new Color(0.75f, 0, 1, 1));
		screen.Texture = bufferedImage.texture;
	}

	public override void _Process(double delta)
	{
		QueueRedraw();
	}
	public override void _PhysicsProcess(double delta)
    {
    }

	public override void _Draw()
	{
		//fillObject3D(canvasCalculation.cube, new Color(0.75f, 0, 1, 1));
		//drawObject3D(canvasCalculation.cube);
		fillRasterizedObject3D(canvasCalculation.cube, new Color(0.75f, 0, 1, 1));
		fillRasterizedObject3D(canvasCalculation.cube2, new Color(0, 1, 0, 1));
		bufferedImage.screenToImage();
		bufferedImage.updateTexture();
		screen.Texture = bufferedImage.texture;
		bufferedImage.fillImage(bufferedImage.backgroundColor);
	}

	public void drawPolygon(Polygon polygon)
	{       
		
		if(canvasCalculation.culling(polygon) < 0)
		{
			Polygon canvasPolygon = canvasCalculation.canvasPolygon(polygon);
			DrawLine(canvasCalculation.pointTo2D(canvasPolygon.pointA), canvasCalculation.pointTo2D(canvasPolygon.pointB), 
			Colors.Green, 1.0f);
			DrawLine(canvasCalculation.pointTo2D(canvasPolygon.pointA), canvasCalculation.pointTo2D(canvasPolygon.pointC), 
			Colors.Green, 1.0f);
			DrawLine(canvasCalculation.pointTo2D(canvasPolygon.pointB), canvasCalculation.pointTo2D(canvasPolygon.pointC), 
			Colors.Green, 1.0f); 
		}  
			
	}

	public void fillPolygon(Polygon polygon, Color color)
	{       
		
		if(canvasCalculation.culling(polygon) < 0)
		{
			Polygon canvasPolygon = canvasCalculation.canvasPolygon(polygon);
			Vector2[] points = {canvasCalculation.pointTo2D(canvasPolygon.pointA), canvasCalculation.pointTo2D(canvasPolygon.pointB), canvasCalculation.pointTo2D(canvasPolygon.pointC)};
			DrawColoredPolygon(points, canvasCalculation.lightColor(polygon, color));
		}
	}

	public void fillRasterizedPolygon(Polygon polygon, Color color)
	{       
		
		if(canvasCalculation.culling(polygon) < 0)
		{
			Polygon canvasPolygon = canvasCalculation.canvasPolygon(polygon);
			Vector2[] points = {canvasCalculation.pointTo2D(canvasPolygon.pointA), canvasCalculation.pointTo2D(canvasPolygon.pointB), canvasCalculation.pointTo2D(canvasPolygon.pointC)};
			Vector3 vectorOA = polygon.pointA - canvasCalculation.camera.pointObserver;
			Vector3 vectorOB = polygon.pointB - canvasCalculation.camera.pointObserver;
			Vector3 vectorOC = polygon.pointC - canvasCalculation.camera.pointObserver;
			float distanceA = vectorOA.Length();
			float distanceB = vectorOB.Length();
			float distanceC = vectorOC.Length();
			bufferedImage.fillRasterizedTriangle(points[0], points[1], points[2], distanceA, distanceB, distanceC, canvasCalculation.lightColor(polygon, color));
		}
	}

	public void fillRasterizedObject3D(Object3D object3D, Color color)
	{
		foreach(Polygon polygon in object3D.polygons)
		{
			fillRasterizedPolygon(polygon, color);
		}
	}

	public void fillObject3D(Object3D object3D, Color color)
	{
		foreach(Polygon polygon in object3D.polygons)
		{
			fillPolygon(polygon, color);
		}
	}

	public void drawObject3D(Object3D object3D)
	{
		foreach(Polygon polygon in object3D.polygons)
		{
			drawPolygon(polygon);
		}
	}
}
