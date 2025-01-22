using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupControls : MonoBehaviour
{
    public DynamicPlayer dynamicPlayer;
    public GameObject layersPanel;
    public GameObject partsPanel;
    public GameObject errorPanel;
    public GameObject introPanel;

    void Start()
    {
        Setup();
    }

    void Setup()
    {
        introPanel.SetActive(false);

        if (dynamicPlayer)
        { 
            if(!dynamicPlayer.musicSet)
            {
                layersPanel.SetActive(false);
                partsPanel.SetActive(false);
                errorPanel.SetActive(true);
                return;
            }

            if (dynamicPlayer.musicSet.partSet == true)
            {
                layersPanel.SetActive(false);
                partsPanel.SetActive(true);
                errorPanel.SetActive(false);
                return;
            }

            if (dynamicPlayer.musicSet.partSet == false)
            {
                layersPanel.SetActive(true);
                partsPanel.SetActive(false);
                errorPanel.SetActive(false);
                return;
            }
        }

    }
}
