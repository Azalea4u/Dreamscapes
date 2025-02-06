using UnityEngine;

public class SRC_AudioManager : MonoBehaviour
{
    public static SRC_AudioManager instance;
    public AudioSource MusicSource;
    public AudioSource SfxSource;

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

    public void PlaySFX(AudioClip clip)
    {
        SfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void SetVolume(float volume)
    {
        MusicSource.volume = volume;
        SfxSource.volume = volume;
    }
}
