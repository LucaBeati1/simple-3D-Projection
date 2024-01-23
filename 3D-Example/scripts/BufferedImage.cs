using Godot;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;

public partial class BufferedImage : RefCounted
{
	public int width, height;
	public Pixel[,] screen;
	public Image image;
	public ImageTexture texture;
	public Color backgroundColor;

	public BufferedImage(int width, int height, Color backgroundColor)
	{
		image = Image.Create(width, height, true, Image.Format.Rgbaf);
		fillImage(backgroundColor);
		screen = new Pixel[width,height];
		initiateScreen();
		clearScreen();
		texture = ImageTexture.CreateFromImage(image);
	}

	public void fillImage(Color color)
	{
		image.Fill(color);
	}

	public void initiateScreen()
	{
		for(int x = 0; x < screen.GetLength(0); x++)
		{
			for(int y = 0; y < screen.GetLength(1); y++)
			{
				screen[x,y] = new Pixel();
			}
		}
	}

	public void clearScreen()
	{
		for(int x = 0; x < screen.GetLength(0); x++)
		{
			for(int y = 0; y < screen.GetLength(1); y++)
			{
				screen[x,y].color = backgroundColor;
				screen[x,y].depth = 99999;
			}
		}
	}

	public void screenToImage()
	{
		for(int x = 0; x < screen.GetLength(0); x++)
		{
			for(int y = 0; y < screen.GetLength(1); y++)
			{
				image.SetPixel(screen[x,y].x, screen[x,y].y, screen[x,y].color);
			}
		}
	}

	public void updateTexture()
	{
		texture.Update(image);
		clearScreen();
	}

	public void fillRasterizedTriangle(Vector2 pointA, Vector2 pointB, Vector2 pointC, float distanceA, float distanceB, float distanceC, Color color)
	{
		int yMin = (int)Math.Min(pointA.Y, Math.Min(pointB.Y, pointC.Y));
		int yMax = (int)Math.Max(pointA.Y, Math.Max(pointB.Y, pointC.Y));

		List<Pixel> pixels = getRasterizedTriangle(pointA, pointB, pointC, distanceA, distanceB, distanceC);

		for(int y = yMin; y <= yMax; y++)
		{
			Pixel[] xIntersections = getXIntersections(y, pixels);
			float distance = xIntersections[1].x - xIntersections[0].x;
			float t;
			float depth;

			for(int x = xIntersections[0].x; x <= xIntersections[1].x; x++)
				{
					t = (x - xIntersections[0].x) / distance;
					depth = xIntersections[0].depth * (1 - t) + xIntersections[1].depth * t;
					if(screen[x,y].depth > depth)
					{
						screen[x,y].x = x;
						screen[x,y].y = y;
						screen[x,y].color = color;
						screen[x,y].depth = depth;
					}
				}
		}
	}

	public Pixel[] getXIntersections(int y, List<Pixel> pixels)
	{
		Pixel[] result = new Pixel[2];
		
		foreach(Pixel pixel in pixels)
		{
			if((int)pixel.y == y)
			{
				result[0] = pixel;
				result[1] = pixel;
				break;
			}
		}
		foreach(Pixel pixel in pixels)
		{
			if((int)pixel.y == y)
			{
				if(pixel.x < result[0].x)
				{
					result[0] = pixel;
				}
				else if(pixel.x > result[1].x)
				{
					result[1] = pixel;
				}
			}
		}
		pixels = pixels.Except(result).ToList();

		return result;
	}

	public void drawRasterizedTriangle(Vector2 pointA, Vector2 pointB, Vector2 pointC, float distanceA, float distanceB, float distanceC, Color color)
	{
		List<Pixel> pixels = getRasterizedTriangle(pointA, pointB, pointC, distanceA, distanceB, distanceC);

		foreach(Pixel pixel in pixels)
		{
			image.SetPixel(pixel.x, pixel.y, color);
		}
		updateTexture();
	}

	public List<Pixel> getRasterizedTriangle(Vector2 pointA, Vector2 pointB, Vector2 pointC, float distanceA, float distanceB, float distanceC)
	{
		List<Pixel> pixels = new List<Pixel>();

		pixels.AddRange(getRasterizedLine(pointA, pointB, distanceA, distanceB));
		pixels.AddRange(getRasterizedLine(pointA, pointC, distanceA, distanceC));
		pixels.AddRange(getRasterizedLine(pointB, pointC, distanceB, distanceC));

		return pixels;
	}

	public List<Pixel> getRasterizedLine(Vector2 pointA, Vector2 pointB, float distanceA, float distanceB)
	{
		List<Pixel> pixels = new List<Pixel>();
		Vector2 vectorAB = pointB - pointA;
		float lineLength = vectorAB.Length();
		float t;

		for(int step = 0; step <= lineLength; step++)
		{
			t = step / lineLength;
			pixels.Add(getRasterizedPoint(pointA, pointB, t, distanceA, distanceB));
		}

		return pixels;
	}

	public Pixel getRasterizedPoint(Vector2 pointA, Vector2 pointB, float t, float distanceA, float distanceB)
	{
		Pixel pixel = new Pixel();
		pixel.x = (int)(pointA.X * (1 - t) + pointB.X * t);
		pixel.y = (int)(pointA.Y * (1 - t) + pointB.Y * t);
		pixel.depth = distanceA * (1 - t) + distanceB * t;

		return pixel;
	}
}
