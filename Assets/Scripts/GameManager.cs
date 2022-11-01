using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public static event Action<int> OnTileCreated;
	public static event Action OnResetTilesState;

	IMovementCommand movementCommand;

	[SerializeField] private GameObject gameObjectGrid;
	[SerializeField] private Button buttonRestart;
	[SerializeField] private Button buttonUndo;
	[SerializeField] private TMP_Text textGameOver;
	[SerializeField] private TMP_Text textScore;
	[SerializeField] private TMP_Text textBestScore;

	public int Score { get; set; }
	private int bestScore;

#region IsGameOver 
	Grid grid;
	private Transform currentTile;
	private Transform tileBelow;
	private Transform tileBeside;
	int maxChildCount;
#endregion


	private void OnEnable()
	{
		buttonRestart.onClick.AddListener(RestartScene);
		buttonUndo.onClick.AddListener(UndoMove);
	}

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		Score = 0;
		bestScore = PlayerPrefs.GetInt("Best Score");
		textBestScore.text = "BEST: " + bestScore;
		grid = GridManager.Instance.grid;
		maxChildCount = grid.Width * grid.Height;
		OnTileCreated?.Invoke(2);
	}

	private void Update()
	{
		if (!IsGameOver())
		{
			UserInput();
		} 
		else
		{
			print("GameOver");
			if (Score > bestScore)
			{
				PlayerPrefs.SetInt("Best Score", Score);
			}
			textGameOver.gameObject.SetActive(true);
		}
	}

	private void UserInput()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			movementCommand = new MovementCommand(Vector2.up);
			movementCommand.Execute();
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			movementCommand = new MovementCommand(Vector2.down);
			movementCommand.Execute();
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			movementCommand = new MovementCommand(Vector2.left);
			movementCommand.Execute();
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			movementCommand = new MovementCommand(Vector2.right);
			movementCommand.Execute();
		}
		OnResetTilesState?.Invoke();
	}

	private void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	private void UndoMove()
	{
		CommandManager.Instance.UndoCommand();
	}
	public void SetTextScore()
	{
		textScore.text = "SCORE: " + Score.ToString();
	}

	private bool IsGameOver()
	{
		if (gameObjectGrid.transform.childCount < maxChildCount)
			return false;

		for (int i = 0; i < grid.Width; i++)
		{
			for (int j = 0; j < grid.Height; j++)
			{
				currentTile = grid.Get(i, j);
				tileBelow = null;
				tileBeside = null;

				if (j != 0)
				{
					tileBelow = grid.Get(i, j - 1);
				}
				if (i != grid.Width - 1)
				{
					tileBeside = grid.Get(i + 1, j);
				}
				if (tileBeside != null)
				{
					if (currentTile.GetComponent<Tile>().tileValue == tileBeside.GetComponent<Tile>().tileValue)
						return false;
				}
				if (tileBelow != null)
				{
					if (currentTile.GetComponent<Tile>().tileValue == tileBelow.GetComponent<Tile>().tileValue)
						return false;
				}
			}
		}
		return true;
	}
}
