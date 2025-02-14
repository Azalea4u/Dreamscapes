using UnityEngine;
using UnityEngine.UI;

public class SCR_TogglePauseMenu : MonoBehaviour
{
    public static SCR_TogglePauseMenu instance;

    [SerializeField] GameObject PauseMenu_UI;

    public void Open_PauseMenu()
    {
        SRC_AudioManager.instance.PlaySFX("Pause_SFX");
        PauseMenu_UI.SetActive(true);
        GameManager.instance.PauseGame(true);
    }

    public void Close_PauseMenu()
    {
        SRC_AudioManager.instance.PlaySFX("Resume_SFX");

        PauseMenu_UI.SetActive(false);
        GameManager.instance.PauseGame(false);
    }
    
    public void Load_MainMenu()
    {
		GameManager.instance.PauseGame(false);
        SRC_AudioManager.instance.ChangeSceneWithMusic(SCR_Loader.scenes.SCN_MainMenu, "MainTheme_Music");
    }

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
}
