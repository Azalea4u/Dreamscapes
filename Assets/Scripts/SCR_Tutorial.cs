using TMPro;
using UnityEditor;
using UnityEngine;

public class SCR_Tutorial : MonoBehaviour {
	[SerializeField] GameObject tutorialContainer;
	[SerializeField] string tutorialText;

	[Header("Pause")]
	[SerializeField] private GameObject Pause_BTN;

	void Start() {
		Pause_BTN.SetActive(false);
		GameManager.instance.PauseGame(true);
		//Time.timeScale = 0;
		tutorialContainer.SetActive(true);
		displayText(tutorialText);
	}

	private void displayText(string text) {
		tutorialContainer.GetComponentInChildren<TextMeshProUGUI>().text = text;
	}

	public void close() {
		tutorialContainer.SetActive(false);
		Pause_BTN.SetActive(true);
		GameManager.instance.PauseGame(false);
		//Time.timeScale = 1;
	}
}
