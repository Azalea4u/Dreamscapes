using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public delegate void ButtonUpdate();
    public static event ButtonUpdate OnButtonUpdate;
    private static int selectedChannel;

    public static int SelectedChannel
    {
        set {
            selectedChannel = value;
            OnButtonUpdate();
        }
        get { 
            return selectedChannel; 
        }
    }
}
