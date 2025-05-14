using UnityEngine;

/// <summary>
/// SceneMusicPlayer is a class that plays
/// the scene's music if the game is not inside
/// a cut scene.
/// </summary>
public class SceneMusicPlayer : MonoBehaviour
{
    [SerializeField] public string trackName;
    [SerializeField] public bool playTrackImmediately;

    private bool _musicStarted;

    public void Awake()
    {
        PlayMusic();
    }

    public void Update()
    {
        if(_musicStarted)
            return;

        PlayMusic();
    }

    /// <summary>
    /// Plays music based on the variables <c>trackName</c> and
    /// <c>playTrackImmediately</c>
    /// </summary>
    private void PlayMusic()
    {
        if(CanStartMusicPlayer())
        {
            _musicStarted = true;
            AudioManager.Instance.PlayMusic(trackName, playTrackImmediately);
        }
        else
            AudioManager.Instance.StopCurrentMusic(true);
    }

    /// <summary>
    /// Boolean method that checks if we can play music.
    /// </summary>
    /// <returns><c>TRUE</c> if we can play music. <c>FALSE</c> if otherwise.</returns>
    private bool CanStartMusicPlayer()
    {
        if(GameManager.Instance.PlayerState.Equals(PlayerState.CUT_SCENE))
            return false;

        return true;
    }
}