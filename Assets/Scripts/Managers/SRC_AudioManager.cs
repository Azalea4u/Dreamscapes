using System;
using UnityEngine;

public class SRC_AudioManager : MonoBehaviour
{
    public static SRC_AudioManager instance;

    public SCR_Sound[] Music_Audios, SFX_Audios;
    public AudioSource Music_Source, SFX_Source;

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
            Debug.Log(sound + " not found");
        }
        else
        {
            Music_Source.clip = sound.clip;
            Music_Source.Play();
        }
    }

    public void PlaySFX(string name)
    {
        SCR_Sound sound = Array.Find(SFX_Audios, x => x.Name == name);

        if (sound == null)
        {
            Debug.Log(sound + " not found");
        }
        else
        {
            SFX_Source.clip = sound.clip;
            SFX_Source.Play();
        }
    }
}
