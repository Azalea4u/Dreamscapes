using TMPro;
using UnityEngine;

public class Score : MonoBehaviour {
	int score = 0;
	TextMeshProUGUI text;

	void Start() {
		text = GetComponentInChildren<TextMeshProUGUI>();
		text.text = "Score: " + score;
	}

	public void ChangeScore(bool add, int amount) {
		if (add) score += amount; else score -= amount;
		text.text = "Score: " + score;
	}
}