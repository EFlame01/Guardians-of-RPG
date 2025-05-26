using System.Collections;
using UnityEngine;

public class NPCPathFinder : MonoBehaviour
{
    [SerializeField] public WayPoint[] _wayPoints;
    [SerializeField] private float _speed;
    [SerializeField] private float _waitTime;
    [SerializeField] public PlayerSprite _npcSprite;

    private Vector3 _startPosition;
    private int _wayPointIndex;
    private WalkCycleState _walkCycleState;
    private bool _waiting;

    public void Start()
    {
        _wayPointIndex = 0;
        _walkCycleState = WalkCycleState.WALKING;
        _waiting = false;
    }

    public void Update()
    {
        _startPosition = transform.position;

        switch(_walkCycleState)
        {
            case WalkCycleState.WALKING:
                TravelToWayPoint();
                if(MadeItToWayPoint())
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

    private void TravelToWayPoint()
    {
        if(_walkCycleState.Equals(WalkCycleState.CANNOT_MOVE) || _walkCycleState.Equals(WalkCycleState.WAITING))
            return;
        
        _npcSprite.PerformWalkAnimation(GetDirectionString());
        transform.position = Vector2.MoveTowards(_startPosition, _wayPoints[_wayPointIndex].Position, Time.fixedDeltaTime * _speed);
    }

    private bool MadeItToWayPoint()
    {
        //TODO: placeholder logic. find better way to test if npc made it to waypoint. - Ese Omene
        return Vector3.Distance(transform.position, _wayPoints[_wayPointIndex].Position) < 0.005f;
    }

    private IEnumerator WaitToTravel()
    {
        if(_waiting)
        {
            _waiting = false;
            float waitTime = (int)Random.Range(_waitTime * 0.5f, _waitTime);
            yield return new WaitForSeconds(waitTime);
            if(!_walkCycleState.Equals(WalkCycleState.CANNOT_MOVE))
                _walkCycleState = WalkCycleState.WALKING;
        }
    }

    private void GetNextWayPoint()
    {
        _npcSprite.PerformIdleAnimation(_wayPoints[_wayPointIndex++].Direction);
        if(_wayPointIndex >= _wayPoints.Length)
            _wayPointIndex = 0;
    }

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

    public void OnCollisionEnter2D(Collision2D collider2D)
    {
        if(!collider2D.gameObject.tag.Equals("Player"))
            return;
        _npcSprite.PerformIdleAnimation(_wayPoints[_wayPointIndex].Direction);
        _walkCycleState  =  WalkCycleState.CANNOT_MOVE;
        _waiting = false;
    }

    public void OnCollisionExit2D(Collision2D collider2D)
    {
        if(!collider2D.gameObject.tag.Equals("Player"))
            return;
        _npcSprite.PerformIdleAnimation(_wayPoints[_wayPointIndex].Direction);
        _walkCycleState  =  WalkCycleState.WAITING;
        _waiting = true;
    }

    public void OnCollisionStay2D(Collision2D collider2D)
    {
        if(!collider2D.gameObject.tag.Equals("Player"))
            return;
        _walkCycleState  =  WalkCycleState.CANNOT_MOVE;
        _waiting = false;
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(!collider2D.gameObject.tag.Equals("Player"))
            return;
        _npcSprite.PerformIdleAnimation(_wayPoints[_wayPointIndex].Direction);
        _walkCycleState  =  WalkCycleState.CANNOT_MOVE;
        _waiting = false;
    }

    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if(!collider2D.gameObject.tag.Equals("Player"))
            return;
        _npcSprite.PerformIdleAnimation(_wayPoints[_wayPointIndex].Direction);
        _walkCycleState  =  WalkCycleState.WAITING;
        _waiting = true;
    }

    public void OnTriggerStay2D(Collider2D collider2D)
    {
        if(!collider2D.gameObject.tag.Equals("Player"))
            return;
        _walkCycleState  =  WalkCycleState.CANNOT_MOVE;
        _waiting = false;
    }
}