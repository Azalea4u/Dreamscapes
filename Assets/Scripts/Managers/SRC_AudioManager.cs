using UnityEngine;

public class SRC_AudioManager : MonoBehaviour
{
    public static SRC_AudioManager instance;

    [Header("Game States")]
    public AudioSource WinGame_SFX;
    public AudioSource GameOver_SFX;
    public AudioSource Pause_SFX;
    public AudioSource Resume_SFX;

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

    public void PauseGame_SFX()
    {
        Pause_SFX.Play();
    }

    public void ResumeGame_SFX()
    {
        Resume_SFX.Play();
    }

    public void GameWon_SFX()
    {
        WinGame_SFX.Play();
    }

    public void GameOver_SFX()
    {
        WinGame_SFX.Play();
    }



}
