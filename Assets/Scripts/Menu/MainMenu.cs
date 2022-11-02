using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private TMP_Text textBestScore;
	[SerializeField] private Button buttonPlay;
	[SerializeField] private Button buttonExit;
	[SerializeField] private GameObject panelLoading; 

	private void OnEnable()
	{
		buttonPlay.onClick.AddListener(() =>
		{
			StartCoroutine(LoadSceneAsync(1));
		});

		buttonExit.onClick.AddListener(() =>
		{
			Application.Quit();
		});
	}

	private void Start()
	{
		textBestScore.text = "BEST SCORE: " + PlayerPrefs.GetInt("Best Score").ToString();
	}

	private IEnumerator LoadSceneAsync(int index)
	{
		panelLoading.SetActive(true);

		yield return new WaitForSeconds(3);

		AsyncOperation op = SceneManager.LoadSceneAsync(index);
	}
}
