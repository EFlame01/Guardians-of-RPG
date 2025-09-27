using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sound is a class that stores the information
/// needed to control a piece of audio.
/// </summary>
[System.Serializable]
public class Sound
{
    //public variables
    public string Name;
    public AudioClip Clip;

    [Range(0f, 1f)] public float Volume;
    [Range(0f, 1f)] public float Pitch;
    public bool Loop;
    public bool AddSource;

    [HideInInspector]
    public AudioSource Source;
}