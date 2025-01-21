using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
    public int channelID;
    public Text sliderLabel;
    public DynamicPlayer dynamicPlayer;

    void Start()
    {
        if (dynamicPlayer.musicSet && channelID < dynamicPlayer.musicSet.audioClips.Length)
        {
            sliderLabel.text = dynamicPlayer.musicSet.audioClips[channelID].name + " Volume";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float newVolume)
    {
        dynamicPlayer.SetSourceVolume(channelID, newVolume);
    }
}
