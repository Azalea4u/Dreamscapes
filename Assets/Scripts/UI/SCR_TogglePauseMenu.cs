using UnityEngine;

public class SCR_TogglePauseMenu : MonoBehaviour
{
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
        SCR_Loader.Load(SCR_Loader.scenes.SCN_MainMenu);
    }
}
