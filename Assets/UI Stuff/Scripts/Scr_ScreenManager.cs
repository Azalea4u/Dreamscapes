using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_ScreenManager : MonoBehaviour {
    public static Scr_ScreenManager instance;

	private void Awake()
    {
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

    public void MainMenu()
    {
        SRC_AudioManager.instance.ChangeSceneWithMusic(SCR_Loader.scenes.SCN_MainMenu, "MainTheme_Music");
    }

    // OnClick methods for the MainMenu Scene
    #region MainMenu Button
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
    #endregion
}