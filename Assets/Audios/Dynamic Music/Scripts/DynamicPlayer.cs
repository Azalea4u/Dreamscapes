using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DynamicPlayer : MonoBehaviour
{
    public bool playOnStart = true;
    public float fadeTime = 1;
    public MusicSet musicSet;
    public AudioMixerGroup mixerGroupOutput;

    AudioSource[] channels;

    void Awake()
    {
        Setup();
        if(playOnStart && musicSet)
        {
            PlayMusic();
        }
    }

    void Setup()
    { 
        if(musicSet)
        {
            channels = new AudioSource[musicSet.audioClips.Length];

            for(int i=0;  i < musicSet.audioClips.Length; i++)
            {
                channels[i] = gameObject.AddComponent<AudioSource>();
                channels[i].playOnAwake = false;
                channels[i].loop = true;
                channels[i].clip = musicSet.audioClips[i];
                if(mixerGroupOutput)
                {
                    channels[i].outputAudioMixerGroup = mixerGroupOutput;
                }
                if(musicSet.partSet && i > 0)
                {
                    channels[i].volume = 0;
                }
            }
        }
    }

    public void PlayMusic()
    { 
        foreach(AudioSource audioSource in channels)
        {
            audioSource.PlayScheduled(AudioSettings.dspTime + 0.2f);
        }
    }

    public void StopMusic()
    {
        foreach (AudioSource audioSource in channels)
        {
            audioSource.Stop();
        }
    }

    public void SetSourceVolume(int sourceID, float newVolume)
    {
        channels[sourceID].volume = newVolume;   
    }

    public void SwitchParts(int partID)
    {
        StopAllCoroutines();
        for (int i = 0; i < channels.Length; i++)
        {
            StartCoroutine(FadeChannel(channels[i], i == partID ? 1 : 0));
        }
    }

    IEnumerator FadeChannel (AudioSource channel, float targetVolume)
    {
        float time = 0;
        float start = channel.volume;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            channel.volume = Mathf.Lerp(start, targetVolume, time / fadeTime);
            yield return null;
        }
        channel.volume = targetVolume;
        yield break;
    }
}
