using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileManager : MonoBehaviour
{
	public static TileManager Instance { get; private set; }

	private GridManager gridManager;
	private GameManager gameManager;

	private readonly float newScale = 0.98f;

	private void OnEnable()
	{
		GameManager.OnTileCreated += OnTileCreatedHandler;
		GameManager.OnResetTilesState += OnResetTilesStateHandler;
		MovementCommand.OnTileCreated += OnTileCreatedHandler;
	}

	private void OnDisable()
	{
		GameManager.OnTileCreated -= OnTileCreatedHandler;
		GameManager.OnResetTilesState -= OnResetTilesStateHandler;
		MovementCommand.OnTileCreated -= OnTileCreatedHandler;
	}

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		gridManager = GridManager.Instance;
		gameManager = GameManager.Instance;
	}

	private void OnTileCreatedHandler(int count)
	{
		GenerateNewTile(count);
	}

	// Set isMergedThisTurn variable to false for all tiles
	private void OnResetTilesStateHandler()
	{
		foreach (Transform item in transform)
		{
			item.GetComponent<Tile>().isMergedThisTurn = false;
		}
	}


	public bool CombineTiles(Transform movingTile, Vector2 destinationPos)
	{
		Vector2 pos = movingTile.position;
		Transform collidingTile = gridManager.grid.Get((int)pos.x, (int)pos.y);

		int movingTileValue = movingTile.GetComponent<Tile>().tileValue;
		int collidingTileValue = collidingTile.GetComponent<Tile>().tileValue;

		if (movingTileValue == collidingTileValue
			&& !movingTile.GetComponent<Tile>().isMergedThisTurn
			&& !collidingTile.GetComponent<Tile>().isMergedThisTurn)
		{
			Destroy(movingTile.gameObject);
			Destroy(collidingTile.gameObject);

			gridManager.grid.Set((int)pos.x, (int)pos.y, null);

			string newTileName = "Tile-" + movingTileValue * 2;
			GameObject newTile = Instantiate(Resources.Load(newTileName, typeof(GameObject)), pos, Quaternion.identity) as GameObject;
			newTile.transform.parent = transform;
			newTile.GetComponent<Tile>().isMergedThisTurn = true;

			// merge animation
			newTile.transform.DOScale(newScale, 0.2f).SetEase(Ease.OutBack);

			gridManager.UpdateGrid();

			// increase score
			gameManager.Score += movingTileValue * 2;
			gameManager.SetTextScore();
			
			return true;
		}
		return false;
	}


	/**
	 * TODO: Object pooling can use instead of Resources.Load
	 **/
	private void GenerateNewTile(int count)
	{
		GameObject newTile;
		Vector2 newTileLocation;
		string tileName = "Tile-2";

		for (int i = 0; i < count; i++)
		{
			// chance of generate 4 is 20%
			float random = Random.Range(0, 1);
			if (random >= 0.8f) tileName = "Tile-4";

			newTileLocation = GridManager.Instance.GetEmptyRandomTileLocation();
			newTile = Instantiate(Resources.Load(tileName, typeof(GameObject)), newTileLocation, Quaternion.identity) as GameObject;
			newTile.transform.parent = transform;

			// new tile animation
			newTile.transform.DOScale(newScale, 0.2f);
		}

		GridManager.Instance.UpdateGrid();
	}
}
