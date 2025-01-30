using TMPro;
using UnityEditor;
using UnityEngine;

public class SCR_Tutorial : MonoBehaviour {
	[SerializeField] GameObject tutorialContainer;
	[SerializeField] string tutorialText;

	void Start() {
		Time.timeScale = 0;
		tutorialContainer.SetActive(true);
		displayText(tutorialText);
	}

	private void displayText(string text) {
		tutorialContainer.GetComponentInChildren<TextMeshProUGUI>().text = text;
	}

	public void close() {
		tutorialContainer.SetActive(false);
		Time.timeScale = 1.0f;
	}
}
