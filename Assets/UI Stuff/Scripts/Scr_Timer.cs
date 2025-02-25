using TMPro;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour {
    public float timeAmount = 0;
    float time = 0;
    bool runTime = false;
    TextMeshProUGUI text;

    void Start() {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Time: " + timeAmount;
        StartTime();
    }

    void Update() {
        if (runTime) {
            time -= Time.deltaTime;
			text.text = "Time: " + time;

			if (time <= 0) { 
                EndTime();
            }
        }
    }

    public void StartTime() {
        time = timeAmount;
		text.text = "Time: " + timeAmount;
		runTime = true;
    }

    public void EndTime() {
        runTime = false;
    }
}