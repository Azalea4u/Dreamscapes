using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    [SerializeField] private bool IsGamePaused = false;
    [SerializeField] private float TotalPlayTime = 0.0f;

    [Header("Mini-Games Data")]
    [SerializeField] private string MainMenu;
    [SerializeField] private string[] MiniGames_Scenes; // list of mini-games scenes
    [SerializeField] private int CurrentMiniGameIndex = 0;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Load_MainMenu()
    {
        SCR_Loader.Load(SCR_Loader.scenes.SCN_MainMenu);
    }

    public void StartMiniGame(int index)
    {
        if (index >= 0 && index < MiniGames_Scenes.Length)
        {
            CurrentMiniGameIndex = index;
        }
    }

    public void NextMiniGame()
    {
        CurrentMiniGameIndex++;
        if (CurrentMiniGameIndex < MiniGames_Scenes.Length)
        {
            StartMiniGame(CurrentMiniGameIndex);
        }
        else
        {
            Debug.Log("All mini-games completed!");
            EndGame();
        }
    }

    public void TogglePause()
    {
        IsGamePaused = !IsGamePaused;
        Time.timeScale = IsGamePaused ? 0 : 1;
        Debug.Log("Game " + (IsGamePaused ? "Paused" : "Resumed"));
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