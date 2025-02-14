using UnityEngine;
using UnityEngine.UI;

public class SCR_TogglePauseMenu : MonoBehaviour
{
    public static SCR_TogglePauseMenu instance;

    [SerializeField] GameObject PauseMenu_UI;

    public void Open_PauseMenu()
    {
        PauseMenu_UI.SetActive(true);
        GameManager.instance.PauseGame(true);
    }

    public void Close_PauseMenu()
    {
        PauseMenu_UI.SetActive(false);
        GameManager.instance.PauseGame(false);
    }
    
    public void Load_MainMenu()
    {
		GameManager.instance.PauseGame(false);
        SCR_Loader.Load(SCR_Loader.scenes.SCN_MainMenu);
	}

    public void StartOver_Spaceship()
    {
        GameManager.instance.PauseGame(false);
        SCR_Loader.Load(SCR_Loader.scenes.SCN_SpaceshipScene);
    }
    
    public void StartOver_Octopus()
    {
        GameManager.instance.PauseGame(false);
        SCR_Loader.Load(SCR_Loader.scenes.SCN_OctopusShooter);
    }

    public void StartOver_FindDragon()
    {
        GameManager.instance.PauseGame(false);
        SCR_Loader.Load(SCR_Loader.scenes.SCN_FindDragonLuigi);
    }

    public void StartOver_Archeology()
    {
        GameManager.instance.PauseGame(false);
        SCR_Loader.Load(SCR_Loader.scenes.SCN_ArcheologyMinigame);
    }
}
