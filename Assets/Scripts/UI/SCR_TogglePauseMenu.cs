using UnityEngine;
using UnityEngine.UI;

public class SCR_TogglePauseMenu : MonoBehaviour
{
    public static SCR_TogglePauseMenu instance;

    [SerializeField] GameObject PauseMenu_UI;

    // OnClick for the PauseBTN to SHOW the PauseMenu_UI
    public void Open_PauseMenu()
    {
        SRC_AudioManager.instance.PlaySFX("Pause_SFX");
        PauseMenu_UI.SetActive(true);
        GameManager.instance.PauseGame(true);
    }

    // OnClick for the PauseBTN to CLOSE the PauseMenu_UI
    public void Close_PauseMenu()
    {
        SRC_AudioManager.instance.PlaySFX("Resume_SFX");

        PauseMenu_UI.SetActive(false);
        GameManager.instance.PauseGame(false);
    }
    
    // Loads up the main menu and makes sure to unpause the game so when entering another mini-game, the game will automatically work as intended
    public void Load_MainMenu()
    {
        GameManager.instance.PauseGame(false);
        SRC_AudioManager.instance.ChangeSceneWithMusic(SCR_Loader.scenes.SCN_MainMenu, "MainTheme_Music");
    }

    // Methods attached to the StartOver btns to reload that same scene
    #region StartOver_Methods
    public void StartOver_Spaceship()
    {
        Scr_ScreenManager.instance.RocketClick();
    }
    
    public void StartOver_Octopus()
    {
        Scr_ScreenManager.instance.OctopusClick();
    }

    public void StartOver_FindDragon()
    {
        Scr_ScreenManager.instance.DragonClick();
    }

    public void StartOver_Archeology()
    {
        Scr_ScreenManager.instance.ArcheologyClick();
    }
    #endregion
}
