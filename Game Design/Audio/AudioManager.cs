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
    //Serialized variables
    [Header("Current Track")]
    [SerializeField] private string _currentMusic;

    [Header("Sound Effects")]
    [SerializeField] private Sound[] _sfxList;

    [Header("Music")]
    [SerializeField] private Sound[] _musicList;

    [Header("Timeline SFX")]
    [SerializeField] private AudioSource[] _timelineSfxList;

    //private variables
    private AudioSource _audioSource1;
    private AudioSource _audioSource2;
    private AudioSource _soundSource;
    private Dictionary<string, Sound> _audioDictionary;

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
        //checks if sfx is valid
        if (!IsSFXValid(name))
            return;

        //get the sfx from the audio dictionary
        Sound sound = _audioDictionary[name];

        //play the sound effect via the _soundSource
        _soundSource.clip = sound.Clip;
        _soundSource.pitch = sound.Pitch;
        _soundSource.loop = sound.Loop;

        if (sound.Source && !sound.Source.isPlaying)
            sound.Source.Play();
        else if (!sound.Source && !_soundSource.isPlaying)
            _soundSource.Play();
    }

    /// <summary>
    /// Plays the name of the sound effect
    /// while pausing the current music 
    /// for a specific amount of time.
    /// </summary>
    /// <param name="name">Name of sound effect</param>
    /// <param name="duration">Length of time music is paused</param>
    public IEnumerator PlaySoundEffect2(string name, float duration)
    {
        //pause music
        yield return PauseMusic();

        //play sound effect
        PlaySoundEffect(name);

        //wait for few seconds
        yield return new WaitForSeconds(duration);

        //resume music
        ResumeMusic();
    }

    /// <summary>
    /// Stops the name of a sound effect.
    /// </summary>
    /// <param name="name">The name of the sound effect</param>
    public void StopSoundEffect(string name)
    {
        //checks if sfx is valid
        if (!IsSFXValid(name))
            return;

        //get the sfx from the audio dictionary
        Sound sound = _audioDictionary[name];

        //stop sound effect if sound is found
        if (sound != null)
        {
            if (sound.Source)
                sound.Source.Stop();
            else
                _soundSource.Stop();
        }
    }

    /// <summary>
    /// Plays the name of the song either gradually
    /// or immediately.
    /// </summary>
    /// <param name="name">The name of the song</param>
    /// <param name="immediately">Determines whether to play the song gradually or immediately</param>
    public void PlayMusic(string name, bool immediately)
    {
        //checks if music is valid to play
        if (!IsAudioValid(name) || IsCurrentMusicPlaying(name))
            return;

        //get music from audio dictionary
        Sound music = _audioDictionary[name];
        _currentMusic = name;

        //set up audio source
        _audioSource1.clip = music.Clip;
        _audioSource1.pitch = music.Pitch;
        _audioSource1.loop = true;

        //set volume either immediately or gradually
        if (immediately)
            _audioSource1.volume = music.Volume * GameManager.Instance.GameVolume;
        else
        {
            _audioSource1.volume = 0f;
            StartCoroutine(StartFade(1f, _audioSource1.volume, music.Volume * GameManager.Instance.GameVolume, _audioSource1));
        }

        //play music while volume is increasing
        _audioSource1.Play();
    }

    /// <summary>
    /// Stops the current song from playing either
    /// gradually or immediately.
    /// </summary>
    /// <param name="immediately">Determines whether to stop the song gradually or immediately</param>
    public void StopCurrentMusic(bool immediately)
    {
        //checks if music is valid to stop
        if (!IsAudioValid(_currentMusic) || !IsCurrentMusicPlaying(_currentMusic))
            return;

        //set volume either immediately or gradually
        if (!immediately)
            StartCoroutine(StartFade(1f, _audioSource1.volume, 0f, _audioSource1));
        else
        {
            _audioSource1.Stop();
            _audioSource1.volume = 0f;
        }

        //update current music to nothing
        _currentMusic = null;
    }

    /// <summary>
    /// Changes the volume in the game.
    /// </summary>
    public void AdjustVolume()
    {
        //check if current music is valid
        if (!IsAudioValid(_currentMusic))
            return;

        //get music from audio dictionary
        Sound music = _audioDictionary[_currentMusic];

        //adjusting volume for current music
        StartCoroutine(StartFade(0.1f, _audioSource1.volume, music.Volume * GameManager.Instance.GameVolume, _audioSource1));

        //adjusting volume for timeline sfx
        foreach (AudioSource sfx in _timelineSfxList)
            sfx.volume *= GameManager.Instance.GameVolume;
    }

    /// <summary>
    /// Creates a cross fade with the current music and
    /// the new music to be played.
    /// </summary>
    /// <param name="trackName">The name of the new music to be played</param>
    public IEnumerator BlendMusic(string trackName)
    {
        if (IsAudioValid(trackName) && !IsCurrentMusicPlaying(trackName))
        {
            //get new music from audio dictionary
            Sound music = _audioDictionary[trackName];
            _currentMusic = music.Name;

            //set up new audio source
            _audioSource2.clip = music.Clip;
            _audioSource2.pitch = music.Pitch;
            _audioSource2.loop = true;

            //start coroutine to blend audio tracks (crossfade)
            StartCoroutine(StartFade(1f, _audioSource1.volume, 0f, _audioSource1));
            StartCoroutine(StartFade(1f, 0f, music.Volume * GameManager.Instance.GameVolume, _audioSource2));

            //play new music while cross fade is in effect
            _audioSource2.Play();

            //stop old music once cross fade ends
            while (_audioSource1.volume != 0)
                yield return null;
            _audioSource1.Stop();

            //swap audio source values 
            // (this is so that the current music playing is attached to _audioSource1)
            (_audioSource2, _audioSource1) = (_audioSource1, _audioSource2);
        }
    }

    /// <summary>
    /// Method used for other classes without
    /// MonoBehaviour to start the coroutine
    /// BlendMusic 
    /// </summary>
    public void BlendMusic2(string trackName)
    {
        StartCoroutine(BlendMusic(trackName));
    }

    /// <summary>
    /// Pauses the current music in the game.
    /// </summary>
    public IEnumerator PauseMusic()
    {
        //checks if music is valid to stop
        if (IsAudioValid(_currentMusic) && IsCurrentMusicPlaying(_currentMusic))
        {
            //start coroutine to fade music off
            StartCoroutine(StartFade(1f, _audioSource1.volume, 0f, _audioSource1));

            //pause audio once volume is at 0
            while (_audioSource1.volume != 0)
                yield return null;
            _audioSource1.Pause();
        }
    }

    /// <summary>
    /// Resumes the current music in the game.
    /// </summary>
    public void ResumeMusic()
    {
        //check if audio is valid
        if (!IsAudioValid(_currentMusic))
            return;

        //get music from audio dictionary
        Sound music = _audioDictionary[_currentMusic];

        //unpause and fade music in
        _audioSource1.UnPause();
        StartCoroutine(StartFade(1f, 0f, music.Volume * GameManager.Instance.GameVolume, _audioSource1));
    }

    /// <summary>
    /// Updates the list of sound effects in the current scene
    /// for the Timeline.
    /// </summary>
    /// <param name="timelineSfxList">the list of sound effects in the timeline</param>
    public void UpdateSFXList(AudioSource[] timelineSfxList)
    {
        _timelineSfxList = timelineSfxList;
    }

    /// <summary>
    /// Initializes the audio dictionary.
    /// </summary>
    private void InitAudioDictionary()
    {
        //init variables
        _audioSource1 = gameObject.AddComponent<AudioSource>();
        _audioSource2 = gameObject.AddComponent<AudioSource>();
        _soundSource = gameObject.AddComponent<AudioSource>();
        _audioDictionary = new Dictionary<string, Sound>();

        //add music to audio dictionary
        foreach (Sound music in _musicList)
            _audioDictionary[music.Name] = music;

        //configure and add sfx to audio dictionary
        foreach (Sound sound in _sfxList)
        {
            if (sound.AddSource)
            {
                sound.Source = gameObject.AddComponent<AudioSource>();
                sound.Source.clip = sound.Clip;
                sound.Source.volume = sound.Volume;
                sound.Source.pitch = sound.Pitch;
                sound.Source.loop = sound.Loop;
            }
            _audioDictionary[sound.Name] = sound;
        }

        //configure variables
        // _musicList = null;
        // _sfxList = null;

        _audioSource1.playOnAwake = false;
        _audioSource2.playOnAwake = false;
        _soundSource.playOnAwake = false;
    }

    /// <summary>
    /// Corrutine that fades music either in our out.
    /// </summary>
    /// <param name="duration">Amount of time the fade should last</param>
    /// <param name="startVolume">Start volume</param>
    /// <param name="targetVolume">End volume</param>
    private IEnumerator StartFade(float duration, float startVolume, float targetVolume, AudioSource audioSource)
    {
        //init variables
        float currentTime = 0f;

        //lerp the volume from start to target until current time == duration
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }

        //if target volume was set to 0, this is an extra check to ensure audio source
        // volume was set to 0
        if (targetVolume <= 0f)
            audioSource.volume = 0f;
    }

    /// <summary>
    /// Determines if a sfx should be played based on
    /// if the game settings and if it's in the audio 
    /// dictionary. This uses the helper method <c>IsAudioValid</c>
    /// </summary>
    /// <param name="name">name of the sound effect</param>
    /// <returns>TRUE if the sfx is valid. FALSE otherwise.</returns>
    private bool IsSFXValid(string name)
    {
        //if sound effects are off, return
        if (!GameManager.Instance.GameSFX)
            return false;

        return IsAudioValid(name);
    }

    /// <summary>
    /// Checks if the audio is valid based on if it's 
    /// in the audio dictionary.
    /// </summary>
    /// <param name="name">name of the audio</param>
    /// <returns>TRUE if the audio is valid. FALSE otherwise.</returns>
    private bool IsAudioValid(string name)
    {
        //if name of sfx does not exist, return false
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("WARNING: the name of this sfx is null or has a length of 0...");
            return false;
        }

        if (!_audioDictionary.ContainsKey(name))
        {
            Debug.LogWarning($"WARNING: the name {name} does not exist in the audio dictionary...");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if the current music that is being played
    /// matches the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the music</param>
    /// <returns>TRUE if names match. FALSE otherwise.</returns>
    private bool IsCurrentMusicPlaying(string name)
    {
        if (_currentMusic == null)
            return false;
        return _audioSource1.isPlaying && _currentMusic.Equals(name);
    }
}