using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    [SerializeField] public bool IsGamePaused = false;

    public float timeSinceTouched;

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

    private void Update()
    {
        // An easy way to leave the build
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        // Makes sure if on MainMenu, that it will unpause/unfreeze the page
        if (SceneManager.GetActiveScene().name == "SCN_MainMenu")
        {
            PauseGame(false);
            timeSinceTouched = 0;
        }
        else if ((SceneManager.GetActiveScene().name != "SCN_MainMenu") || (SceneManager.GetActiveScene().name != "SCN_Loading"))
        {
            if (Input.touchCount == 0)
            {
                timeSinceTouched = Time.timeSinceLevelLoad;
                Debug.Log(timeSinceTouched);

                if (timeSinceTouched >= 30)
                {
                    BackToMainMenu();
                }
            }
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

    // in case some bozo walks away in the middle of the game
    public void BackToMainMenu()
    {
        timeSinceTouched = 0;
        Scr_ScreenManager.instance.MainMenu();
    }
}