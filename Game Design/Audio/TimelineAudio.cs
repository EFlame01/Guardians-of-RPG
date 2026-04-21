using UnityEngine;

/// <summary>
/// TimelineAudio is a class that updates the sound
/// effects for the timelines (cutscenes) using the
/// <c>AudioManager</c> 
/// </summary>
public class TimelineAudio : MonoBehaviour
{
    [SerializeField] private AudioSource[] _audioSources;

    public void Start()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.volume *= GameManager.Instance.GameVolume;
        }

        AudioManager.Instance.UpdateSFXList(_audioSources);
    }
}