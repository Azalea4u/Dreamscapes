using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_ScreenManager : Singleton<Scr_ScreenManager> {
    [SerializeField] GameObject titleS;
    [SerializeField] GameObject playS;
    [SerializeField] GameObject pauseS;
    [SerializeField] GameObject menuS;

	bool pause = false;
    bool menu = false;

	void Start() {
        
    }

    void Update() {
        
    }

    public void PlayClick() {
        menuS.SetActive(true);
        menu = true;
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

    public void MenuClick() {
        menu = !menu;
        if (menu) {
            menuS.SetActive(true);
            titleS.SetActive(false);
            return;
        }
        menuS.SetActive(false);
        menu = false;
        titleS.SetActive(true);
    }

    public void ArcheologyClick() {
        SceneManager.LoadScene("SCN_ArcheologyMinigame");
        Destroy(gameObject);
    }

    public void DragonClick() {
        SceneManager.LoadScene("");
    }

    public void RocketClick() {
        SceneManager.LoadScene("");
    }
}