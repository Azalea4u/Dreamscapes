using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    [SerializeField] public bool IsGamePaused = false;

    private void Awake()
    {
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
        if (SceneManager.GetActiveScene().name == "SCN_MainMenu")
        {
            PauseGame(false);
        }
    }

    // Pauses the Game from any script
    public void PauseGame(bool pauseGame)
    {
        IsGamePaused = pauseGame;
        Time.timeScale = IsGamePaused ? 0 : 1;
        //Debug.Log("Game " + (IsGamePaused ? "Paused" : "Resumed"));
    }

    public IEnumerator Add_WaitTime(float time)
    {
        yield return new WaitForSeconds(time);
    }
}