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
    public void PlayClick()
    {
        playS.SetActive(true);
        titleS.SetActive(false);
    }

    public void QuitClick()
    {
        pauseS.SetActive(false);
        titleS.SetActive(true);
    }

    public void PauseClick()
    {
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

    public void MenuClick()
    {
        menu = !menu;
        if (menu) {
            menuS.SetActive(true);
            return;
        }
        menuS.SetActive(false);
    }

    public void RocketClick()
    {
        GameManager.instance.PauseGame(false);
        SRC_AudioManager.instance.ChangeSceneWithMusic(SCR_Loader.scenes.SCN_SpaceshipScene, "Spaceship_Music");
    }

    public void ArcheologyClick()
    {
        GameManager.instance.PauseGame(false);
        SRC_AudioManager.instance.ChangeSceneWithMusic(SCR_Loader.scenes.SCN_ArcheologyMinigame, "Arch_Music");
	}

    public void DragonClick()
    {
        GameManager.instance.PauseGame(false);
        SRC_AudioManager.instance.ChangeSceneWithMusic(SCR_Loader.scenes.SCN_FindDragonLuigi, "Finding_Music");
	}

    public void OctopusClick()
    {
        GameManager.instance.PauseGame(false);
        SRC_AudioManager.instance.ChangeSceneWithMusic(SCR_Loader.scenes.SCN_OctopusShooter, "Octopus_Music");
    }
}