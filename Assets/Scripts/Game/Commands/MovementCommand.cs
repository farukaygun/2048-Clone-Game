using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCommand : IMovementCommand
{
	public static event Action<int> OnTileCreated;
	public static event Action OnTilesMoving;
	private Vector2 direction;
	private Grid grid;

	public MovementCommand(Vector2 direction)
	{
		this.direction = direction;
	}

	public void Execute()
	{
		MoveAllTiles(direction);
	}

	// TODO: Undo command
	public void Undo()
	{
		Debug.Log("Undo");
	}

	private void MoveAllTiles(Vector2 direction)
	{
		grid = GridManager.Instance.grid;
		int tilesMovedCount = 0;
		GridManager.Instance.UpdateGrid();

		if (direction == Vector2.up)
		{
			for (int i = 0; i < grid.Width; i++)
			{
				for (int j = grid.Height - 1; j >= 0; j--)
				{
					if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
				}
			}
		}
		else if (direction == Vector2.down)
		{
			for (int i = 0; i < grid.Width; i++)
			{ 
				for (int j = 0; j < grid.Height; j++)
				{ 
					if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
				}
			}
		}
		else if (direction == Vector2.left)
		{
			for (int i = 0; i < grid.Width; i++)
			{ 
				for (int j = 0; j < grid.Height; j++)
				{	if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
				}
			}
		}
		else if (direction == Vector2.right)
		{
			for (int i = grid.Width - 1; i >= 0; i--)
			{
				for (int j = 0; j < grid.Height; j++)
				{ 
					if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
				}
			}
		}

        OnTilesMoving?.Invoke();

        // generate new tile after move tiles
        if (tilesMovedCount != 0)
			OnTileCreated?.Invoke(1); // generate new 1 tile 
	}

	private bool MoveTile(Transform tile, Vector2 direction)
	{
		Vector3 startPos = tile.localPosition;
		Vector3 ghostTilePos = tile.localPosition;
		Vector3 previousPosition;

		tile.GetComponent<Tile>().startingPosition = startPos;

		// until the tile goes as far as it can go
		while (true)
		{
			// set position to next node
			ghostTilePos += (Vector3)direction;
			previousPosition = ghostTilePos - (Vector3)direction;

			// is new node inside grid.
			if (grid.IsInsideGrid(ghostTilePos))
			{
				// is new node valid? Is there any other tile at new node.
				if (grid.IsDirectionValid(ghostTilePos))
				{
					// if there is no tile at new node then upgrade grid.
					tile.GetComponent<Tile>().movePosition = ghostTilePos;
					grid.Set((int)previousPosition.x, (int)previousPosition.y, null);
					grid.Set((int)ghostTilePos.x, (int)ghostTilePos.y, tile);
				}
				else
				{
					// if these tiles can't merged.
					if (!TileManager.Instance.JoinTiles(tile, ghostTilePos, previousPosition))
					{
						ghostTilePos -= (Vector3)direction;
						tile.GetComponent<Tile>().movePosition = ghostTilePos;

						// if tile's substitution is 0.
						if (ghostTilePos == startPos)
							return false; // tile didn't move.
						else return true; // tile moved.
					}
				}
			}
			else
			{
				ghostTilePos -= (Vector3)direction;
				tile.GetComponent<Tile>().movePosition = ghostTilePos;
				if (ghostTilePos == startPos)
					return false;
				else return true;
			}
		}
	}
}
