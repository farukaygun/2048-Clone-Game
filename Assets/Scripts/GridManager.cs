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
		grid = new Grid(4, 4);
	}

	public void UpdateGrid()
	{
		// ? IDK Array.Clear() maybe works for this
		// TODO: Try Array.Clear(grid, 0, grid.Length)
		for (int i = 0; i < grid.Width; i++)
		{
			for (int j = 0; j < grid.Height; j++)
			{
				var node = grid.Get(i, j);
				if (node && node.parent == transform)
					grid.Set(i, j, null);
			}
		}

		// Get children's transform of gameobject and set to grid.
		foreach (Transform item in transform)
		{
			Vector2 tilePos = new Vector2(Mathf.Round(item.position.x), Mathf.Round(item.position.y));
			grid.Set((int)tilePos.x, (int)tilePos.y, item);
		}
	}

	/**
	 * TODO: Object pooling can use instead of Resources.Load
	 **/
	public void GenerateNewTile(int count)
	{
		GameObject newTile;
		Vector2 newTileLocation;
		string tileName = "Tile-2";

		for (int i = 0; i < count; i++)
		{
			// chance of generate 4 is 20%
			float random = Random.Range(0, 1);
			if (random >= 0.8f) tileName = "Tile-4";

			newTileLocation = GetEmptyRandomTileLocation();
			newTile = Instantiate(Resources.Load(tileName, typeof(GameObject)), newTileLocation, Quaternion.identity) as GameObject;
			newTile.transform.parent = transform;
		}

		GridManager.Instance.UpdateGrid();
	}

	private Vector2 GetEmptyRandomTileLocation()
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
