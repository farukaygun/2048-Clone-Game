using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class creates a grid accorting to the given width, height and cellsize in constructor.
/// </summary>
public class Grid
{
	private int width;
	private int height;
	private float cellOffset = 0.5f; // set node position to center of square
	private readonly Transform[,] gridArray;

	public float cellSize;


	public Grid(int width, int height, float cellSize = 1) {
		this.width = width;
		this.height = height;
		this.cellSize = cellSize;

		gridArray = new Transform[width, height];

		DisplayGrid();
	}

	/**
	 * Draw grid lines in the screen.
	 **/
	private void DisplayGrid()
	{
		// display created grid
		for (int x = 0; x < gridArray.GetLength(0); x++)
		{
			for (int y = 0; y < gridArray.GetLength(1); y++)
			{
				Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
				Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
			}

			Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
			Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
		}
	}


	/**
	 * Returns the position of the grid's node according to the given indexes.
	 **/
	public Vector2 GetWorldPosition(int x, int y)
	{
		Vector2 pos = new Vector3(x, y) * cellSize;
		return new Vector2(pos.x + cellOffset, pos.y + cellOffset);
	}

	/**
	 * Width of grid
	 **/
	public int GetWidth()
	{
		return gridArray.GetLength(0);
	}

	/**
	 * Height of grid
	 **/
	public int GetHeight()
	{
		return gridArray.GetLength(1);
	}

	/**
	 * Set a Gameobject grid's node according to the given indexes.
	 **/
#nullable enable
	public void Set(int x, int y, Transform? transform)
	{
		if (x < width && x >= 0 && y < width && y >= 0)
		{
			gridArray[x, y] = transform;
		}
	}
#nullable disable

	/**
	 * Return node of grid according to the given indexes.
	 **/
	public Transform Get(int x, int y)
	{
		return gridArray[x, y];
	}
}
