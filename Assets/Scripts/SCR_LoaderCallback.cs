using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_LoaderCallback : MonoBehaviour {
    private bool IsFirstUpdate = true;

    private void Update()
    {
        if (IsFirstUpdate)
        {
            IsFirstUpdate = false;

            if (SRC_AudioManager.instance != null)
            {
                SRC_AudioManager.instance.OnLoadingScreenShown();
            }
        }
    }
}