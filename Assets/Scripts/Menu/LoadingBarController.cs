using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarController : MonoBehaviour
{
	[SerializeField] private Slider slider;
	[SerializeField] private TMP_Text textLoadingBarValue;

	public void OnLoadingBarChanged(float value)
	{
		textLoadingBarValue.text = value.ToString() + "%";
		slider.value = value;
	}
}
