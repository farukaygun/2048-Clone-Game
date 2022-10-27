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

	private int width = 3;
	private int height = 3;
	private Grid grid;
	private int i = 0, j = 0;


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
		{

		} 
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{

		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{

		}
	}

	/**
	 * TODO: Object pooling can use instead of Resources.Load
	 **/
	private void GenerateNewTile(int count)
	{
		GameObject newTile;
		Vector2 newTileLocation;
		string tileName = "Tile-2";

		for (i = 0; i < count; i++)
		{
			// chance of generate 4 is 10%
			float random = Random.Range(0, 1);
			if (random >= 0.9f) tileName = "Tile-4";

			newTileLocation = GetEmptyRandomTileLocation();
			newTile = Instantiate(Resources.Load(tileName, typeof(GameObject)), newTileLocation, Quaternion.identity) as GameObject;
			newTile.transform.parent = transform;
		}

		UpdateGrid();
	}

	void UpdateGrid()
	{
		// ? IDK Array.Clear() maybe works for this
		// TODO: Try Array.Clear(grid, 0, grid.Length)
		for (i = 0; i < width; i++)
		{
			for (j = 0; j < height; j++)
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

		for (i = 0; i < width; i++)
			for (j = 0; j < height; j++)
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
