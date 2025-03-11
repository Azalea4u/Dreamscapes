using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SRC_AudioManager : MonoBehaviour
{
    public static SRC_AudioManager instance;
    public static SCR_MainMenuBTN mmb;

    public SCR_Sound[] Music_Audios, SFX_Audios;
    public AudioSource Music_Source, SFX_Source;

    public float fadeDuration = 0.75f; // Adjust as needed

    private string nextMusicName = "";
    public bool playing = true;
    [SerializeField] public bool isMusicFading = false;

    private void Awake()
    {
        //Singleton
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

    private void Start()
    {
        // play when loading MainMenu Scene
        PlayMusic("MainTheme_Music");
        Music_Source.mute = false;
        
    }

    // Sets the Music Audio Source to a certain AudioClip
    public void PlayMusic(string name)
    {
        SCR_Sound sound = Array.Find(Music_Audios, x => x.Name == name);

        if (sound == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            Music_Source.clip = sound.clip;
            Music_Source.loop = true;
            Music_Source.Play();
        }
    }
    public bool PauseMusic(bool Paused)
    {
        if(!Paused)
        {
            Music_Source.mute = false;
        }
        else
        {
            Music_Source.mute = true;
        }

        return playing = !Paused;
    }

    // Sets the SFX Audio Source to a certain AudioClip
    public void PlaySFX(string name)
    {
        SCR_Sound sound = Array.Find(SFX_Audios, x => x.Name == name);

        if (sound == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            SFX_Source.clip = sound.clip;
            SFX_Source.Play();
        }
    }

    // Use this Method to change the bg music while changing scenes
    public void ChangeSceneWithMusic(SCR_Loader.scenes targetScene, string newMusic)
    {
        if (!isMusicFading)
        {
            nextMusicName = newMusic;
            isMusicFading = true;
            SCR_Loader.Load(targetScene);
        }
    }

    public void OnLoadingScreenShown()
    {
        StartCoroutine(FadeOutMusic());
    }

    // When swapping scenes, the music will fade out
    private IEnumerator FadeOutMusic()
    {
        float startVolume = Music_Source.volume;
        while (Music_Source.volume > 0)
        {
            Music_Source.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        Music_Source.Stop();
        Music_Source.volume = startVolume; // Reset volume for next music

        SCR_Loader.ContinueToTargetScene(); // Now load the actual scene
    }

    // When the scene is loaded, checks if nextMusicName isn't null before fading in the new bg music
    public void OnSceneLoaded()
    {
        if (!string.IsNullOrEmpty(nextMusicName))
        {
            StartCoroutine(FadeInMusic(nextMusicName));
        }
    }

    // After music has fade out, the next scene's music will fade in
    private IEnumerator FadeInMusic(string musicName)
    {
        PlayMusic(musicName);
        Music_Source.volume = 0;
        float targetVolume = 1.0f;

        while (Music_Source.volume < targetVolume)
        {
            Music_Source.volume += targetVolume * Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }

        isMusicFading = false;
    }
}
