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
	[SerializeField] private LoadingBarController loadingBarController;

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
		textBestScore.text = "BEST\n" + PlayerPrefs.GetInt("Best Score").ToString();
		
	}

	// Added delay for showing loading bar and screen.
	private IEnumerator LoadSceneAsync(int index)
	{
		int loadingValue = 0;

		AsyncOperation operation = SceneManager.LoadSceneAsync(index);
		operation.allowSceneActivation = false;

		panelLoading.SetActive(true);

		while(!operation.isDone)
		{
			loadingBarController.OnLoadingBarChanged(loadingValue);
			loadingValue += 1;

			if (loadingValue >= 100)
			{
				operation.allowSceneActivation = true;
			}

			yield return new WaitForSeconds(0.05f);
		}
	}
}
