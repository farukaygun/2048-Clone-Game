using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovementManager : MonoBehaviour
{
	private Grid grid;
	private void OnEnable()
	{
		GameManager.OnTileMovement += OnMovementHandler;
	}

	private void OnDisable()
	{
		GameManager.OnTileMovement -= OnMovementHandler;
	}

	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GridManager.Instance.grid != null);
		grid = GridManager.Instance.grid;
	}

	private void OnMovementHandler(Vector2 direction)
	{
		MoveAllTiles(direction);
	}

	// TODO: Move the method to Tile.cs with Observer pattern
	private void MoveAllTiles(Vector2 direction)
	{
		int tilesMovedCount = 0;

		if (direction == Vector2.up)
		{
			for (int i = 0; i < grid.Width; i++)
				for (int j = grid.Height - 1; j >= 0; j--)
					if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
		}
		else if (direction == Vector2.down)
		{
			for (int i = 0; i < grid.Width; i++)
				for (int j = 0; j < grid.Height; j++)
					if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
		}
		else if (direction == Vector2.left)
		{
			for (int i = 0; i < grid.Width; i++)
				for (int j = 0; j < grid.Height; j++)
					if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
		}
		else if (direction == Vector2.right)
		{
			for (int i = grid.Width - 1; i >= 0; i--)
				for (int j = 0; j < grid.Height; j++)
					if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
		}

		// generate new tile after move tiles
		if (tilesMovedCount != 0)
			GridManager.Instance.GenerateNewTile(1);
	}


	// TODO: Could be better method.
	private bool MoveTile(Transform tile, Vector2 direction)
	{
		Vector3 startPos = tile.localPosition;
		Vector2 pos;

		while (true)
		{
			tile.transform.localPosition += (Vector3)direction;
			pos = tile.transform.localPosition;
			if (grid.IsInsideGrid((int)pos.x, (int)pos.y) && grid.IsDirectionValid((int)pos.x, (int)pos.y))
				GridManager.Instance.UpdateGrid();
			else
			{
				tile.transform.localPosition -= (Vector3)direction;
				if (tile.transform.localPosition == startPos)
					return false;
				else return true;
			}
		}
	}
}
