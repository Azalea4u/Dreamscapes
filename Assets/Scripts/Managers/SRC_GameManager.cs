using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    [SerializeField] public bool IsGamePaused = false;
    public float timesinceTouched;

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
        if (SceneManager.GetActiveScene().name == "SCN_MainMenu" || (SceneManager.GetActiveScene().name == "SCN_Loading"))
        {
            PauseGame(false);
            timesinceTouched = 0;
        }
        else if ((SceneManager.GetActiveScene().name != "SCN_MainMenu") || (SceneManager.GetActiveScene().name != "SCN_Loading"))
        {
            GoToMainMenu();
        }

    }

    public void GoToMainMenu()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            //Debug.Log("Mouse Clicked");
            timesinceTouched = 0;
        }
        else //if (Input.touchCount <= 0 || !Input.GetMouseButtonDown(0))
        {
            timesinceTouched += Time.unscaledDeltaTime;
            //Debug.Log((int)timesinceTouched);

            if (timesinceTouched >= 30)
            {
                BackToMainMenu();
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
        Scr_ScreenManager.instance.MainMenu();
        Add_WaitTime(1.0f);
        timesinceTouched = 0;
    }
}