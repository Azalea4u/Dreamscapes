using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_ScreenManager : MonoBehaviour {

    public static Scr_ScreenManager instance;

    [SerializeField] GameObject titleS;
    [SerializeField] GameObject playS;
    [SerializeField] GameObject pauseS;
    [SerializeField] GameObject menuS;

	bool pause = false;
    bool menu = false;

	void Start() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void MenuClick() {
        menu = !menu;
        if (menu) {
            menuS.SetActive(true);
            return;
        }
        menuS.SetActive(false);
    }

    public void RocketClick() {
        //SceneManager.LoadScene("SCN_SpaceshipScene");
        SCR_Loader.Load(SCR_Loader.scenes.SCN_SpaceshipScene);
        Destroy(gameObject);
    }

    public void ArcheologyClick() {
        //SceneManager.LoadScene("SCN_ArcheologyMinigame");
        SCR_Loader.Load(SCR_Loader.scenes.SCN_ArcheologyMinigame);
        Destroy(gameObject);
	}

    public void DragonClick() {
        //SceneManager.LoadScene("SCN_FindDragonLuigi");
        SCR_Loader.Load(SCR_Loader.scenes.SCN_FindDragonLuigi);
        Destroy(gameObject);
	}

    public void OctopusClick() {
        SCR_Loader.Load(SCR_Loader.scenes.Octopus);
        Destroy(gameObject);
    }
}