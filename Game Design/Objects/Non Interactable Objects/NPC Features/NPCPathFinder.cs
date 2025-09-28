using System.Collections;
using UnityEngine;

/// <summary>
/// NPCPathFinder is a class
/// </summary>
public class NPCPathFinder : MonoBehaviour
{
    //Serialized variables
    [SerializeField] private WayPoint[] _wayPoints;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _waitTime;
    [SerializeField] private PlayerSprite _npcSprite;

    //protected variables
    protected Vector3 _startPosition;
    protected int _wayPointIndex;
    protected WalkCycleState _walkCycleState;
    protected bool _waiting;

    public void Start()
    {
        _wayPointIndex = 0;
        _walkCycleState = WalkCycleState.WALKING;
        _waiting = false;
    }

    public void Update()
    {
        _startPosition = transform.position;

        switch (_walkCycleState)
        {
            case WalkCycleState.WALKING:
                TravelToWayPoint();
                if (MadeItToWayPoint())
                {
                    GetNextWayPoint();
                    _walkCycleState = WalkCycleState.WAITING;
                    _waiting = true;
                }
                break;
            case WalkCycleState.WAITING:
                StartCoroutine(WaitToTravel());
                break;
            case WalkCycleState.CANNOT_MOVE:
                break;
        }
    }

    /// <summary>
    /// Checks if the NPC can travel to a certain
    /// WayPoint. If it can, it will use Vector2.MoveTowards
    /// to traverse to that location. This method works 
    /// when it is called in the Update() method (called every frame).
    /// </summary>
    private void TravelToWayPoint()
    {
        if (_walkCycleState.Equals(WalkCycleState.CANNOT_MOVE) || _walkCycleState.Equals(WalkCycleState.WAITING))
            return;

        _npcSprite.PerformWalkAnimation(GetDirectionString());
        transform.position = Vector2.MoveTowards(_startPosition, _wayPoints[_wayPointIndex].Position, Time.fixedDeltaTime * _speed);
    }

    /// <summary>
    /// Checks if NPC has made it
    /// to WayPoint.
    /// </summary>
    /// <returns><c>TRUE</c> if NPC made it to WayPoint. Otherwise it returns <c>FALSE</c></returns>
    private bool MadeItToWayPoint()
    {
        //TODO: placeholder logic. find better way to test if npc made it to waypoint.
        return Vector3.Distance(transform.position, _wayPoints[_wayPointIndex].Position) < 0.005f;
    }

    /// <summary>
    /// After randomly deciding a duration from a 
    /// specified range, this method will set the
    /// NPC's WalkCycleState to CANNOT_MOVE until
    /// the duration is over. Once the time has ran
    /// out, this method sets the NPC's WalkCycleState
    /// to WALKING.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitToTravel()
    {
        if (_waiting)
        {
            _waiting = false;
            float waitTime = (int)Random.Range(_waitTime * 0.5f, _waitTime);
            yield return new WaitForSeconds(waitTime);
            if (!_walkCycleState.Equals(WalkCycleState.CANNOT_MOVE))
                _walkCycleState = WalkCycleState.WALKING;
        }
    }

    /// <summary>
    /// This method indexes the next WayPoint
    /// in the array to go through. If the NPC
    /// has reached the end of the array, then
    /// this metod resets the index to 0.
    /// </summary>
    private void GetNextWayPoint()
    {
        _npcSprite.PerformIdleAnimation(_wayPoints[_wayPointIndex++].Direction);
        if (_wayPointIndex >= _wayPoints.Length)
            _wayPointIndex = 0;
    }

    /// <summary>
    /// This method returns the animation
    /// name associated with the direction
    /// the player is facing.
    /// </summary>
    /// <returns>The name of the animation</returns>
    private string GetDirectionString()
    {
        return _wayPoints[_wayPointIndex].Direction switch
        {
            PlayerDirection.UP => "walk_up",
            PlayerDirection.DOWN => "walk_down",
            PlayerDirection.LEFT => "walk_left",
            PlayerDirection.RIGHT => "walk_right",
            _ => "walk_down",
        };
    }


    //Collision Methods
    public virtual void OnCollisionEnter2D(Collision2D collider2D)
    {
        if (!collider2D.gameObject.CompareTag("Player"))
            return;
        _npcSprite.PerformIdleAnimation(_wayPoints[_wayPointIndex].Direction);
        _walkCycleState = WalkCycleState.CANNOT_MOVE;
        _waiting = false;
    }

    public virtual void OnCollisionExit2D(Collision2D collider2D)
    {
        if (!collider2D.gameObject.CompareTag("Player"))
            return;
        _npcSprite.PerformIdleAnimation(_wayPoints[_wayPointIndex].Direction);
        _walkCycleState = WalkCycleState.WAITING;
        _waiting = true;
    }

    public virtual void OnCollisionStay2D(Collision2D collider2D)
    {
        if (!collider2D.gameObject.CompareTag("Player"))
            return;
        _walkCycleState = WalkCycleState.CANNOT_MOVE;
        _waiting = false;
    }

    public virtual void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (!collider2D.gameObject.CompareTag("Player"))
            return;
        _npcSprite.PerformIdleAnimation(_wayPoints[_wayPointIndex].Direction);
        _walkCycleState = WalkCycleState.CANNOT_MOVE;
        _waiting = false;
    }

    public virtual void OnTriggerExit2D(Collider2D collider2D)
    {
        if (!collider2D.gameObject.CompareTag("Player"))
            return;
        _npcSprite.PerformIdleAnimation(_wayPoints[_wayPointIndex].Direction);
        _walkCycleState = WalkCycleState.WAITING;
        _waiting = true;
    }

    public virtual void OnTriggerStay2D(Collider2D collider2D)
    {
        if (!collider2D.gameObject.CompareTag("Player"))
            return;
        _walkCycleState = WalkCycleState.CANNOT_MOVE;
        _waiting = false;
    }
}