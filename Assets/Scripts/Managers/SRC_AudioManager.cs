using UnityEngine;

public class SRC_AudioManager : MonoBehaviour
{
    public static SRC_AudioManager instance;

    [Header("Game States")]
    public AudioSource WinGame_SFX;
    public AudioSource _GameOver_SFX;

    [Header("UI SFX")]
    public AudioSource Pause_SFX;
    public AudioSource Resume_SFX;

    [Header("Background Music")]
    public AudioSource Archeology_BG;
    public AudioSource Spaceship_BG;
    public AudioSource FindingCharacter_BG;
    public AudioSource Octopus_BG;

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

    #region Game States
    public void GameWon_SFX()
    {
        WinGame_SFX.Play();
    }

    public void GameOver_SFX()
    {
        WinGame_SFX.Play();
    }
    #endregion

    #region UI SFX
    public void PauseGame_SFX()
    {
        Pause_SFX.Play();
    }

    public void ResumeGame_SFX()
    {
        Resume_SFX.Play();
    }

    public void SetMute(bool mute)
    {

    }
    #endregion


}
