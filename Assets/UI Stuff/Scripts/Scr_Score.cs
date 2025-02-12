using TMPro;
using UnityEngine;

public class Score : MonoBehaviour {
	int score = 0;
	TextMeshProUGUI text;

	void Start() {
		text = GetComponentInChildren<TextMeshProUGUI>();
		text.text = "Score \n" + score;
	}

	public void ChangeScore(int amount) {
		score += amount;
		text.text = "Score \n" + score;
	}
}