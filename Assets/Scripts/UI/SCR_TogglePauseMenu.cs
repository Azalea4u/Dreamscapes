using UnityEngine;

public class SCR_TogglePauseMenu : MonoBehaviour
{
    public static SCR_TogglePauseMenu instance;

    [SerializeField] private GameObject PauseMenu_UI;
    [SerializeField] private GameObject Pause_BTN;

    public void Open_PauseMenu()
    {
        PauseMenu_UI.SetActive(true);
        Pause_BTN.SetActive(false);
        GameManager.instance.PauseGame(true);
    }

    public void Close_PauseMenu()
    {
        PauseMenu_UI.SetActive(false);
        Pause_BTN.SetActive(true);
        GameManager.instance.PauseGame(false);
    }
    
    public void Load_MainMenu()
    {
        SCR_Loader.Load(SCR_Loader.scenes.SCN_MainMenu);
    }

    public void StartOver_Spaceship()
    {
        SCR_Loader.Load(SCR_Loader.scenes.SCN_SpaceshipScene);
    }
    
    public void StartOver_Octopus()
    {
        SCR_Loader.Load(SCR_Loader.scenes.SCN_OctopusShooter);
    }

    public void StartOver_FindDragon()
    {
        SCR_Loader.Load(SCR_Loader.scenes.SCN_FindDragonLuigi);
    }

    public void StartOver_Archeology()
    {
        SCR_Loader.Load(SCR_Loader.scenes.SCN_ArcheologyMinigame);
    }
}
