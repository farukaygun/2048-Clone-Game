using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class creates a grid accorting to the given width, height and cellsize.
/// </summary>
public class Grid
{
	public int Width { get; set; }
	public int Height { get; set; }

	private readonly float cellOffset = 0.5f; // set node position to center of square
	private readonly Transform[,] grid;
	private readonly float cellSize;

	public Grid(int width, int height, float cellSize = 1)
	{
		this.Width = width;
		this.Height = height;
		this.cellSize = cellSize;

		grid = new Transform[width, height];

		DisplayGrid();
	}

	/// <summary>
	/// Draw grid lines in the screen.
	/// </summary>
	private void DisplayGrid()
	{
		for (int x = 0; x < grid.GetLength(0) - 1; x++)
		{
			for (int y = 0; y < grid.GetLength(1) - 1; y++)
			{
				Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
				Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
			}

			Debug.DrawLine(GetWorldPosition(0, Height - 1), GetWorldPosition(Width - 1, Height - 1), Color.white, 100f);
			Debug.DrawLine(GetWorldPosition(Width - 1, 0), GetWorldPosition(Width - 1, Height - 1), Color.white, 100f);
		}
	}


	/// <summary>
	/// Returns the position of the grid's node according to the given indexes.
	/// </summary>
	public Vector2 GetWorldPosition(int x, int y)
	{
		Vector2 pos = new Vector3(x, y) * cellSize;
		return new Vector2(pos.x + cellOffset, pos.y + cellOffset);
	}

#nullable enable
	/// <summary>
	/// Set a Gameobject grid's node according to the given indexes.
	/// </summary>
	/// <param name="x"> Row index of grid. </param>
	/// <param name="y"> Column index of grid. </param>
	/// <param name="transform"> Transform object to set. </param>
	public void Set(int x, int y, Transform? transform)
	{
		if (x < Width && x >= 0 && y < Width && y >= 0)
			grid[x, y] = transform;
	}
#nullable disable

	/// <summary>
	/// Return node of grid according to the given indexes.
	/// </summary>
	public Transform Get(int x, int y)
	{
		return grid[x, y];
	}

	/// <summary>
	/// Returns whether the given indexes are nodes of the grid.
	/// </summary>
	public bool IsInsideGrid(Vector2 pos)
	{
		if (pos.x < Width && pos.x >= 0 && pos.y < Height && pos.y >= 0)
			return true;
		return false;
	}

	/// <summary>
	/// Is the direction of the tile valid for movement?
	///	If there is other tile at location return false
	/// </summary>
	public bool IsDirectionValid(Vector2 pos)
	{
		if (grid[(int)pos.x, (int)pos.y] == null)
			return true;
		return false;
	}
}
