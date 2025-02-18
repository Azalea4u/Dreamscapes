using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SRC_AudioManager : MonoBehaviour
{
    public static SRC_AudioManager instance;

    public SCR_Sound[] Music_Audios, SFX_Audios;
    public AudioSource Music_Source, SFX_Source;

    public float fadeDuration = 0.75f; // Adjust as needed

    private string nextMusicName = "";
    private bool isFading = false;

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

    private void Start()
    {
        // play when loading MainMenu Scene
        PlayMusic("MainTheme_Music");
    }

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

    public void ChangeSceneWithMusic(SCR_Loader.scenes targetScene, string newMusic)
    {
        if (!isFading)
        {
            nextMusicName = newMusic;
            isFading = true;
            SCR_Loader.Load(targetScene);
        }
    }

    public void OnLoadingScreenShown()
    {
        StartCoroutine(FadeOutMusic());
    }

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

    public void OnSceneLoaded()
    {
        if (!string.IsNullOrEmpty(nextMusicName))
        {
            StartCoroutine(FadeInMusic(nextMusicName));
        }
    }

    private IEnumerator FadeInMusic(string musicName)
    {
        PlayMusic(musicName);
        Music_Source.volume = 0;
        float targetVolume = 1.0f;

        while (Music_Source.volume < targetVolume)
        {
            Music_Source.volume += targetVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        isFading = false; // Reset flag after fade-in completes
    }
}
