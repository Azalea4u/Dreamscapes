using TMPro;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour {
    public float timeAmount = 0;
    float time = 0;
    bool runTime = false;
    TextMeshProUGUI text;

    void Start() {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Time: " + (int)timeAmount;
        StartTime();
    }

    void Update() {
        if (runTime) {
            time -= Time.deltaTime;
			text.text = "Time: " + (int)timeAmount;

			if (time <= 0) { 
                EndTime();
            }
        }
    }

    public void StartTime() {
        time = timeAmount;
		text.text = "Time: " + (int)timeAmount;
		runTime = true;
    }

    public void EndTime() {
        runTime = false;
    }
}