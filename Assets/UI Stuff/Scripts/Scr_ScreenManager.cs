using UnityEngine;

public class Scr_ScreenManager : Singleton<Scr_ScreenManager> {
    [SerializeField] GameObject titleS;
    [SerializeField] GameObject playS;
    [SerializeField] GameObject pauseS;

	bool pause = false;

	void Start() {
        
    }

    void Update() {
        
    }

    public void PlayClick() {
        playS.SetActive(true);
        titleS.SetActive(false);
    }

    public void QuitClick() {
        pauseS.SetActive(false);
        titleS.SetActive(true);
    }

    public void PauseClick() {
		pause = !pause;
        if (pause) {
            Time.timeScale = 0;
            pauseS.SetActive(true);
            playS.SetActive(false);
        } else { 
            Time.timeScale = 1;
			pauseS.SetActive(false);
			playS.SetActive(true);
		}
	}
}