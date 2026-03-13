using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System.Linq;
using System;

public class TimelineAudio : MonoBehaviour
{
    [SerializeField] private AudioSource[] _audioSources;

    public void Start()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.volume = audioSource.volume * GameManager.Instance.GameVolume;
        }

        AudioManager.Instance.UpdateSFXList(_audioSources);
    }
}