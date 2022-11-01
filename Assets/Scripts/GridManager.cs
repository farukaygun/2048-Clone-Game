using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour 
{
	public static GridManager Instance { get; private set; }
	public Grid grid;

	private void Awake()
	{
		Instance = this;	
	}

	private void Start()
	{
		grid = new(4, 4);
	}

	public void UpdateGrid()
	{
		Transform node;
		Vector2 tilePos;
		// TODO: Try Array.Clear(grid, 0, grid.Length)
		for (int i = 0; i < grid.Width; i++)
		{
			for (int j = 0; j < grid.Height; j++)
			{
				node = grid.Get(i, j);
				if (node && node.parent == transform)
					grid.Set(i, j, null);
			}
		}

		// Get children's transform of gameobject and set to grid.
		foreach (Transform item in transform)
		{
			tilePos = new(Mathf.Round(item.position.x), Mathf.Round(item.position.y));
			grid.Set((int)tilePos.x, (int)tilePos.y, item);
		}
	}

	public Vector2 GetEmptyRandomTileLocation()
	{
		List<Vector2> emptyTileIndexes = new();
		for (int i = 0; i < grid.Width; i++)
			for (int j = 0; j < grid.Height; j++)
				if (grid.Get(i, j) == null)
					emptyTileIndexes.Add(new Vector2(i, j));

		int randomIndex = Random.Range(0, emptyTileIndexes.Count);

		return emptyTileIndexes[randomIndex];
	}
}
