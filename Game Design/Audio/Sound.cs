using UnityEngine;

/// <summary>
/// Sound is a class that stores the information
/// needed to control a piece of audio.
/// </summary>
[System.Serializable]
public class Sound
{
    //public variables
    [Header("General Sound Details")]
    [SerializeField] public string Name;
    [SerializeField] public AudioClip Clip;

    [Header("Advanced Sound Details")]
    [Range(0f, 1f)][SerializeField] public float Volume;
    [Range(0f, 1f)][SerializeField] public float Pitch;
    [SerializeField] public bool Loop;
    [SerializeField] public bool AddSource;

    //public variable
    [HideInInspector]
    public AudioSource Source;
}