using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public bool IsGamePaused = false;
    public float TotalPlayTime = 0.0f;

    [Header("Mini-Games Data")]
    public string[] MiniGames_Scenes; // list of mini-games scenes
    public int CurrentMiniGameIndex = 0;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the GameManager across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Add Pause Button as well
        {
            TogglePause();
        }
    }

    public void StartMiniGame(int index)
    {
        if (index >= 0 && index < MiniGames_Scenes.Length)
        {
            CurrentMiniGameIndex = index;
            // Loading Screen
            // Load Mini-Game Scene
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

    public void EndGame()
    {
        // return to main menu
    }
}
