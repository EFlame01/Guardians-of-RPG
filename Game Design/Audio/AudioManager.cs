using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// AudioManager is a class that is designed to 
/// handle all of the music and sound effects 
/// used in the game.
///</summary>
public class AudioManager : PersistentSingleton<AudioManager>
{
    [Header("Sound Effects")]
    [SerializeField] private Sound[] _soundList;

    [Header("Music")]
    [SerializeField] private Sound[] _musicList;

    private AudioSource _audioSource;
    private AudioSource _soundSource;
    private Dictionary<string, Sound> _audioDictionary;
    private string _currentMusic;

    protected override void Awake()
    {
        base.Awake();
        InitAudioDictionary();
    }

    /// <summary>
    /// Plays the name of a sound effect.
    /// </summary>
    /// <param name="name">The name of the sound effect</param>
    public void PlaySoundEffect(string name)
    {
        if(!GameManager.Instance.GameSFX)
            return;
        
        if(name == null || name.Length <= 0)
            return;
        
        Sound sound = _audioDictionary[name];
        
        if(sound != null)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            float volume = GameManager.Instance.GameVolume * sound.Volume;

            _soundSource.clip = sound.Clip;
            _soundSource.pitch = sound.Pitch;
            _soundSource.loop = sound.Loop;
            
            if(sound.Source && !sound.Source.isPlaying)
                sound.Source.Play();
            else if(!sound.Source && !_soundSource.isPlaying)
                _soundSource.Play();
        }
    }

    /// <summary>
    /// Stops the name of a sound effect.
    /// </summary>
    /// <param name="name">The name of the sound effect</param>
    public void StopSoundEffect(string name)
    {
        Sound sound = _audioDictionary[name];
        
        if(sound == null)
            return;
        
        if(sound.Source)
            sound.Source.Stop();
        else
            _soundSource.Stop();
    }

    /// <summary>
    /// Plays the name of the song either gradually
    /// or immediately.
    /// </summary>
    /// <param name="name">The name of the song</param>
    /// <param name="immediately">Determines whether to play the song gradually or immediately</param>
    public void PlayMusic(string name, bool immediately)
    {
        if(name == null || name.Length == 0)
            Debug.Log(name + " not found. Cannot play music.");
            
        Sound music = _audioDictionary[name];

        if(name == null || music == null || name.Equals(_currentMusic))
            return;


        //play music.
        _currentMusic = name;
        _audioSource.clip = music.Clip;
        _audioSource.pitch = music.Pitch;
        _audioSource.loop = true;
        
        if(immediately)
            _audioSource.volume = music.Volume * GameManager.Instance.GameVolume;
        else
        {
            _audioSource.volume = 0f;
            StartCoroutine(StartFade(1f, _audioSource.volume, music.Volume * GameManager.Instance.GameVolume));
        }
        _audioSource.Play();
    }

    /// <summary>
    /// Plays the name of the song either gradually
    /// or immediately for temporary <paramref name="audioSource"/>
    /// </summary>
    /// <param name="name">The name of the song</param>
    /// <param name="immediately">Determines whether to play the song gradually or immediately</param>
    /// <param name="audioSource">Temporary audio source</param>
    public void PlayMusic(string name, bool immediately, AudioSource audioSource)
    {
        if(name == null)
            Debug.Log(name + " not found. Cannot play music.");
            
        Sound music = _audioDictionary[name];

        if(name == null || music == null || name.Equals(_currentMusic))
            return;

        //play music.
        _currentMusic = name;
        audioSource.clip = music.Clip;
        audioSource.pitch = music.Pitch;
        audioSource.loop = true;
        
        if(immediately)
            audioSource.volume = music.Volume * GameManager.Instance.GameVolume;
        else
        {
            audioSource.volume = 0f;
            StartCoroutine(StartFade(1f, audioSource.volume, music.Volume * GameManager.Instance.GameVolume, audioSource));
        }
        audioSource.Play();
    }

    /// <summary>
    /// Stops the current song from playing either
    /// gradually or immediately.
    /// </summary>
    /// <param name="immediately">Determines whether to stop the song gradually or immediately</param>
    public void StopCurrentMusic(bool immediately)
    {
        if(!_audioSource.isPlaying)
            return;

        if(_currentMusic == null)
            return;

        if(_audioDictionary[_currentMusic] == null)
            return;

        if(!immediately)
            StartCoroutine(StartFade(1f, _audioSource.volume, 0f));
        else
        {
            _audioSource.Stop();
            _audioSource.volume = 0f;
        }

        _currentMusic = null;
    }

    /// <summary>
    /// Stops the current song from playing either
    /// gradually or immediately for temporary
    /// <paramref name="audioSource"/>
    /// </summary>
    /// <param name="immediately">Determines whether to stop the song gradually or immediately</param>
    /// <param name="audioSource">Temporary audio source</param>
    private void StopCurrentMusic(bool immediately, AudioSource audioSource)
    {
        if(!audioSource.isPlaying)
            return;

        if(_currentMusic == null)
            return;

        if(_audioDictionary[_currentMusic] == null)
            return;

        if(!immediately)
            StartCoroutine(StartFade(1f, audioSource.volume, 0f, audioSource));
        else
        {
            audioSource.Stop();
            audioSource.volume = 0f;
        }

        _currentMusic = null;
    }

    public void AdjustVolume()
    {
        if(_currentMusic == null)
            return;
        Sound music = _audioDictionary[_currentMusic];
        StartCoroutine(StartFade(0.1f, _audioSource.volume, music.Volume * GameManager.Instance.GameVolume));
    }

    public void BlendMusic(string trackName)
    {
        if(trackName.Equals(_currentMusic))
            return;

        AudioSource tempAudioSource = _audioSource;
        
        StopCurrentMusic(false, tempAudioSource);
        PlayMusic(trackName, false);
    }

    /// <summary>
    /// Initializes the audio dictionary.
    /// </summary>
    private void InitAudioDictionary()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _soundSource = gameObject.AddComponent<AudioSource>();
        _audioDictionary = new Dictionary<string, Sound>();
        
        foreach(Sound music in _musicList)
        {
            _audioDictionary[music.Name] = music;
        }
        
        foreach(Sound sound in _soundList)
        {
            if(sound.AddSource)
            {
                sound.Source = gameObject.AddComponent<AudioSource>();
                sound.Source.clip = sound.Clip;
                sound.Source.volume = sound.Volume;
                sound.Source.pitch = sound.Pitch;
                sound.Source.loop = sound.Loop;
            }
            _audioDictionary[sound.Name] = sound;
        }

        _musicList = null;
        _soundList = null;

        _audioSource.playOnAwake = false;
        _soundSource.playOnAwake = false;
    }

    /// <summary>
    /// Corrutine that fades music either in our out.
    /// </summary>
    /// <param name="duration">Amount of time the fade should last</param>
    /// <param name="startVolume">Start volume</param>
    /// <param name="targetVolume">End volume</param>
    /// <returns></returns>
    private IEnumerator StartFade(float duration, float startVolume, float targetVolume)
    {
        float currentTime = 0f;

        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime/duration);
            yield return null;
        }
        
        if(targetVolume <= 0f)
            _audioSource.volume = 0f;
    }

    /// <summary>
    /// Coroutine that fades music either in our out for temporary
    /// <paramref name="audioSource"/>
    /// </summary>
    /// <param name="duration">Amount of time the fade should last</param>
    /// <param name="startVolume">Start volume</param>
    /// <param name="targetVolume">End volume</param>
    /// <param name="audioSource">temporary audio source</param>
    /// <returns></returns>
    private IEnumerator StartFade(float duration, float startVolume, float targetVolume, AudioSource audioSource)
    {
        float currentTime = 0f;

        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime/duration);
            yield return null;
        }
        
        if(targetVolume <= 0f)
            audioSource.volume = 0f;
    }
}