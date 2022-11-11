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

		MovementCommand.OnTilesMoving += OnTilesMovingHandler;
	}

	private void OnDisable()
	{
		GameManager.OnTileCreated -= OnTileCreatedHandler;
		GameManager.OnResetTilesState -= OnResetTilesStateHandler;
		MovementCommand.OnTileCreated -= OnTileCreatedHandler;

		MovementCommand.OnTilesMoving -= OnTilesMovingHandler;
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

	private void OnTilesMovingHandler()
	{
		for (int i = 0; i < gridManager.grid.Width; i++)
		{
			for (int j = 0; j < gridManager.grid.Height; j++)
			{
                Transform tile = gridManager.grid.Get(i, j);
                if (tile)
                    StartCoroutine(SlideTile(tile, 10f));
            }
		}
	}


	// TODO: Try to do this with DOTween.
	private IEnumerator SlideTile(Transform tile, float timeScale)
	{
		Tile tileScript = tile.GetComponent<Tile>();
		float progress = 0;
		while (progress <= 1)
		{
			tile.localPosition = Vector2.Lerp(tileScript.startingPosition, tileScript.movePosition, progress);
			progress += Time.deltaTime * timeScale;
			yield return null;
		}

		tile.localPosition = tileScript.movePosition;
		if (tileScript.destroy)
		{
			int movingTileValue = tileScript.tileValue;
			if (tileScript.collidingTile != null)
				Destroy(tileScript.collidingTile.gameObject);

			Destroy(tile.gameObject);

			// create new tile after merge.
			string newTileName = "Tile-" + movingTileValue * 2;
			GameObject newTile = Instantiate(Resources.Load(newTileName, typeof(GameObject)), tile.localPosition, Quaternion.identity) as GameObject;
			newTile.transform.parent = transform;
			newTile.GetComponent<Tile>().isMergedThisTurn = true;
			newTile.GetComponent<Tile>().name = "slideTileNewTile";
			newTile.transform.DOScale(newScale, 0.2f);

			gridManager.grid.Set((int)newTile.transform.localPosition.x, (int)newTile.transform.localPosition.y, newTile.transform);
		}
		yield return null;
	}

	// Set isMergedThisTurn variable to false for all tiles
	private void OnResetTilesStateHandler()
	{
		foreach (Transform item in transform)
		{
			item.GetComponent<Tile>().isMergedThisTurn = false;
		}
	}


	public bool JoinTiles(Transform movingTile, Vector2 ghostTilePosition, Vector2 previousPosition)
	{
		Transform collidingTile = gridManager.grid.Get((int)ghostTilePosition.x, (int)ghostTilePosition.y);
		Tile movingTileScript = movingTile.GetComponent<Tile>();
		Tile collidingTileScript = collidingTile.GetComponent<Tile>();
		

		int movingTileValue = movingTileScript.tileValue;
		int collidingTileValue = collidingTileScript.tileValue;

		if (movingTileValue == collidingTileValue
			&& !movingTileScript.isMergedThisTurn
			&& !collidingTileScript.isMergedThisTurn
			&& !collidingTileScript.willMergeWithCollidingTile)
		{
			movingTileScript.destroy = true;
			movingTileScript.collidingTile = collidingTile;
			movingTileScript.movePosition = ghostTilePosition;

			gridManager.grid.Set((int)previousPosition.x, (int)previousPosition.y, null);
			gridManager.grid.Set((int)ghostTilePosition.x, (int)ghostTilePosition.y, movingTile);

			movingTileScript.willMergeWithCollidingTile = true;

			// increase score
			gameManager.Score += movingTileValue * 2;
			gameManager.SetTextScore();
			
			return true;
		}
		return false;
	}


	/**
	 * TODO: Object pooling may be use instead of Resources.Load
	 **/
	private void GenerateNewTile(int count)
	{
		GameObject newTile;
		Vector2 newTileLocation;
		string tileName = "Tile-2";


		for (int i = 0; i < count; i++)
		{
			// chance of generate 4 is 20%
			float random = Random.Range(0f, 1f);
			if (random >= 0.9f) tileName = "Tile-4";

			newTileLocation = GridManager.Instance.GetEmptyRandomTileLocation();
			newTile = Instantiate(Resources.Load(tileName, typeof(GameObject)), newTileLocation, Quaternion.identity) as GameObject;
			newTile.transform.parent = transform;

            gridManager.grid.Set((int)newTile.transform.localPosition.x, (int)newTile.transform.localPosition.y, newTile.transform);

            // new tile animation
            newTile.transform.DOScale(newScale, 0.2f);
        }
	}
}
