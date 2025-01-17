using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public int channelID;
    public Text buttonLabel;
    public DynamicPlayer dynamicPlayer;
    Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        if (dynamicPlayer.musicSet && channelID < dynamicPlayer.musicSet.audioClips.Length)
        {
            buttonLabel.text = dynamicPlayer.musicSet.audioClips[channelID].name;
        }
        else
        {
            Destroy(gameObject);
        }

        CheckButtonState();
    }

    public void SwitchToPart()
    {
        dynamicPlayer.SwitchParts(channelID);
        ButtonManager.SelectedChannel = channelID;
    }

    void CheckButtonState()
    { 
        if(ButtonManager.SelectedChannel == channelID)
        {
            myButton.interactable = false;
        }
        else
        {
            myButton.interactable = true;
        }
    }

    private void OnEnable()
    {
        ButtonManager.OnButtonUpdate += CheckButtonState;
    }

    private void OnDisable()
    {
        ButtonManager.OnButtonUpdate -= CheckButtonState;
    }
}
