using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputHandler is a class that inherits from the Interact
/// class to handle all of the inputs from the player.
/// 
/// TODO: Implement input for pausing the game.
/// </summary>
public class InputHandler : MonoBehaviour
{
    [SerializeField] private GameObject _menuOption;
    [SerializeField] private PlayerSprite _playerSprite;
    public CharacterPos charPos;

    public InputActionReference Move;
    // public InputAction Pause;

    private Rigidbody2D _rb2D;
    private float _speed;
    private Vector3 _targetPos;
    private Vector2 _velocity;


    private void Start()
    {
        transform.localPosition = Vector3.zero;
        _rb2D = GetComponent<Rigidbody2D>();
        _speed = GameManager.Instance.PlayerSpeed;
        _targetPos = transform.position;
        _playerSprite.PerformIdleAnimation(PlayerSpawn.PlayerDirection);
    }

    private void Update()
    {
        _velocity = Move.action.ReadValue<Vector2>();
        if (_velocity.y != 0)
            _velocity.x = 0;

        _targetPos = transform.position + (Vector3)_velocity;
    }

    private void FixedUpdate()
    {
        if (CanMove())
            ControlMovement();
        else
            _playerSprite.PerformIdleAnimation(PlayerSpawn.PlayerDirection);
    }

    /// <summary>
    /// Opens the menu and changes the
    /// PlayerState to PAUSED to pause the 
    /// game.
    /// </summary>
    public void OnMenuButtonClicked()
    {
        OpenMenu();
        GameManager.Instance.PlayerState = PlayerState.PAUSED;
    }

    /// <summary>
    /// Determines if the player is in a state where they
    /// are eligible to move.
    /// </summary>
    /// <returns>true if the player can move, false otherwise.</returns>
    private bool CanMove()
    {
        return GameManager.Instance.PlayerState == PlayerState.NOT_MOVING || GameManager.Instance.PlayerState == PlayerState.MOVING;
    }

    /// <summary>
    /// Moves and animates the player based on the 
    /// _velocity, _speed, and _targetPos, after which
    /// it will update the waypoint of the player.
    /// </summary>
    public void ControlMovement()
    {
        if (Vector3.Distance(transform.position, _targetPos) > Mathf.Epsilon)
        {
            if (_velocity.y > 0)
            {
                _velocity.y = 1;
                _playerSprite.PerformWalkAnimation("walk_up");
                PlayerSpawn.PlayerDirection = PlayerDirection.UP;
                GameManager.Instance.PlayerState = PlayerState.MOVING;
            }
            else if (_velocity.y < 0)
            {
                _velocity.y = -1;
                _playerSprite.PerformWalkAnimation("walk_down");
                PlayerSpawn.PlayerDirection = PlayerDirection.DOWN;
                GameManager.Instance.PlayerState = PlayerState.MOVING;
            }
            else if (_velocity.x < 0)
            {
                _velocity.x = -1;
                _playerSprite.PerformWalkAnimation("walk_left");
                PlayerSpawn.PlayerDirection = PlayerDirection.LEFT;
                GameManager.Instance.PlayerState = PlayerState.MOVING;
            }
            else if (_velocity.x > 0)
            {
                _velocity.x = 1;
                _playerSprite.PerformWalkAnimation("walk_right");
                PlayerSpawn.PlayerDirection = PlayerDirection.RIGHT;
                GameManager.Instance.PlayerState = PlayerState.MOVING;
            }
            _rb2D.MovePosition(_rb2D.position + _velocity * _speed * Time.fixedDeltaTime);
            charPos.AddWayPoint(_rb2D.position, PlayerSpawn.PlayerDirection);
        }
        else
        {
            _playerSprite.PerformIdleAnimation(PlayerSpawn.PlayerDirection);
            GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
        }

        PlayerSpawn.PlayerPosition = transform.position;
    }

    /// <summary>
    /// Opens the menu and changes the
    /// PlayerState to PAUSED to pause the 
    /// game.
    /// <summary>
    /// <param name="context">The input action's callback context</param>
    private void PauseGame(InputAction.CallbackContext context)
    {
        OpenMenu();
        GameManager.Instance.PlayerState = PlayerState.PAUSED;
    }

    /// <summary>
    /// Instantiates the menu gameobject and keeps
    /// the player in an idle position.
    /// </summary>
    private void OpenMenu()
    {
        Instantiate(_menuOption);
        _playerSprite.PerformIdleAnimation(PlayerSpawn.PlayerDirection);
    }
}