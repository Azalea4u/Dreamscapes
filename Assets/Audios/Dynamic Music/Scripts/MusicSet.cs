using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Music Set")]
public class MusicSet : ScriptableObject
{
    public AudioClip[] audioClips;
    [Tooltip("Check this box to play only one clip at a time (Parts), uncheck this box to allow multiple clips to be played at once (Layers).")]
    public bool partSet;
}
