using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public static event Action<Vector2> OnTileMovement;

	[SerializeField] private Button playAgain;


	private void OnEnable()
	{
		playAgain.onClick.AddListener(RestartScene);
	}

	private void Awake()
	{
		Instance = this;
	}

	private IEnumerator Start()
	{
		// wait until grid initializes.
		yield return new WaitUntil(() => GridManager.Instance != null);
		GridManager.Instance.GenerateNewTile(2);
	}

	private void Update()
	{
		UserInput();
	}

	private void UserInput()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
			OnTileMovement?.Invoke(Vector2.up);
		else if (Input.GetKeyDown(KeyCode.DownArrow))
			OnTileMovement?.Invoke(Vector2.down);
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
			OnTileMovement?.Invoke(Vector2.left);
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			OnTileMovement?.Invoke(Vector2.right);
	}

	private void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
