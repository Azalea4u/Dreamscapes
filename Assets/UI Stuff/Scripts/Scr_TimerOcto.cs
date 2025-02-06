using TMPro;
using UnityEngine;

public class Scr_TimerOcto : MonoBehaviour {
	[SerializeField] float timeAmount = 0;
	[SerializeField] Scr_OctoSpawner[] spawners;
	float time = 0;
	bool runTime = false;

	void Start() {
		StartTime();
	}

	void Update() {
		if (runTime) {
			time -= Time.deltaTime;

			if (time <= 0) {
				EndTime();
			}
		}
	}

	public void StartTime() {
		time = timeAmount;
		runTime = true;
	}

	public void EndTime() {
		foreach (Scr_OctoSpawner spawner in spawners) {
			spawner.Spawn();
		}

		runTime = false;
		Destroy(gameObject);
	}
}