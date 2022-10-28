using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
	[SerializeField] private Button playAgain;

	private int width = 4;
	private int height = 4;
	private Grid grid;


	private void OnEnable()
	{
		playAgain.onClick.AddListener(RestartScene);
	}

	private void Start()
	{
		grid = new Grid(width, height);
		GenerateNewTile(2);
	}

	private void Update()
	{
		UserInput();
	}

	private void UserInput()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
			MoveAllTiles(Vector2.up);
		else if (Input.GetKeyDown(KeyCode.DownArrow))
			MoveAllTiles(Vector2.down);
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
			MoveAllTiles(Vector2.left);
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			MoveAllTiles(Vector2.right);
	}

	// TODO: Move the method to Tile.cs with Observer pattern
	private void MoveAllTiles(Vector2 direction)
	{
		int tilesMovedCount = 0;

		if (direction == Vector2.up)
		{
			for (int i = 0; i < width; i++)
				for (int j = height - 1; j >= 0; j--)
					if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
		}
		else if (direction == Vector2.down)
		{
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
					if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
		}
		else if (direction == Vector2.left)
		{
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
					if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
		}
		else if (direction == Vector2.right)
		{
			for (int i = width - 1; i >= 0; i--)
				for (int j = 0; j < height; j++)
					if (grid.Get(i, j) != null && MoveTile(grid.Get(i, j), direction))
						tilesMovedCount++;
		}

		// generate new tile after move tiles
		if (tilesMovedCount != 0)
			GenerateNewTile(1);
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
			if (grid.IsInsideGrid((int)pos.x, (int)pos.y))
				if (grid.IsDirectionValid((int)pos.x, (int)pos.y))
					UpdateGrid();
				// TODO : Same as following else. Refactor this.
				else
				{
					tile.transform.localPosition -= (Vector3)direction;
					if (tile.transform.localPosition == startPos)
						return false;
					else return true;
				}
			else
			{
				tile.transform.localPosition -= (Vector3)direction;
				if (tile.transform.localPosition == startPos)
					return false;
				else return true;
			}
		}
	}

	/**
	 * TODO: Object pooling can use instead of Resources.Load
	 **/
	private void GenerateNewTile(int count)
	{
		Debug.Log("Count: " + count);
		GameObject newTile;
		Vector2 newTileLocation;
		string tileName = "Tile-2";

		for (int i = 0; i < count; i++)
		{
			Debug.Log(i + " " + tileName);
			// chance of generate 4 is 10%
			float random = Random.Range(0, 1);
			if (random >= 0.9f) tileName = "Tile-4";

			newTileLocation = GetEmptyRandomTileLocation();
			newTile = Instantiate(Resources.Load(tileName, typeof(GameObject)), newTileLocation, Quaternion.identity) as GameObject;
			newTile.transform.parent = transform;
			print(count);
		}

		UpdateGrid();
	}

	void UpdateGrid()
	{
		// ? IDK Array.Clear() maybe works for this
		// TODO: Try Array.Clear(grid, 0, grid.Length)
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
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

	private Vector2 GetEmptyRandomTileLocation()
	{
		List<Vector2> emptyTileIndexes = new List<Vector2>();

		for (int i = 0; i < width; i++)
			for (int j = 0; j < height; j++)
				if (grid.Get(i, j) == null)
					emptyTileIndexes.Add(new Vector2(i, j));

		int randomIndex = Random.Range(0, emptyTileIndexes.Count);

		return emptyTileIndexes[randomIndex];
	}

	private void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
