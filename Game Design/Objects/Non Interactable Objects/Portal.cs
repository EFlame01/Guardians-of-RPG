using UnityEngine;

/// <summary>
/// Portal is a class that when added
/// to an object, allows the player
/// to be transfered to a new scene.
/// </summary>
public class Portal : MonoBehaviour
{
    //Serialized variables
    [SerializeField] private string _sceneName;
    [SerializeField] private Vector3 _scenePosition;
    [SerializeField] private PlayerDirection _direction;
    [SerializeField] private TransitionType _transitionType;
    [SerializeField] private bool _fadeSongOut;

    //private variables
    private bool _traveled;

    /// <summary>
    /// Sends player to a new location.
    /// </summary>
    public void SendToNewLocation()
    {
        //stop current music
        if (_fadeSongOut)
            AudioManager.Instance.StopCurrentMusic(false);

        //add player _direction and position
        GameManager.Instance.PlayerState = PlayerState.TRANSITION;
        PlayerSpawn.PlayerDirection = _direction;
        PlayerSpawn.PlayerPosition = _scenePosition;

        //send to new location
        SceneLoader.walkInAnimation = true;
        SceneLoader.Instance.LoadScene(_sceneName, _transitionType);
    }

    //Collision Methods
    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (!collider2D.gameObject.CompareTag("Player"))
            return;

        if (GameManager.Instance.PlayerState.Equals(PlayerState.TRANSITION))
            return;

        if (!_traveled)
        {
            _traveled = true;
            SendToNewLocation();
        }
    }
}