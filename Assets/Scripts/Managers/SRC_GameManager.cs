using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    [SerializeField] private bool IsGamePaused = false;

    [Header("Mini-Games Data")]
    [SerializeField] private string MainMenu;


    private void Awake()
    {
        // Singleton Pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the GameManager across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager
        }
    }

    public void Load_MainMenu()
    {
        SCR_Loader.Load(SCR_Loader.scenes.SCN_MainMenu);
    }

    public void PauseGame(bool pauseGame)
    {
        IsGamePaused = pauseGame;
        Time.timeScale = IsGamePaused ? 0 : 1;
        Debug.Log("Game " + (IsGamePaused ? "Paused" : "Resumed"));
    }

    public void EndGame()
    {
        // return to main menu
        Load_MainMenu();
    }
}