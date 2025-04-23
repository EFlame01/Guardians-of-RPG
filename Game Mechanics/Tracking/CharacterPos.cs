using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CharacterPos is a class that is used
/// to measure the position of a character
/// and mark their waypoints as they travel
/// across the map.
/// </summary>
public class CharacterPos : MonoBehaviour
{
    [SerializeField] Transform PlayerIndex;
    [SerializeField] public Vector2 Position;
    [SerializeField] public PlayerDirection Direction;

    public List<WayPoint> WayPoints {get; private set;}

    public void Awake()
    {
        InitStartingPoint();
    }

    public void ClearWayPoints()
    {
        WayPoints.Clear();
        InitStartingPoint();
    }

    /// <summary>
    /// Adds a waypoint to the list of waypoints.
    /// </summary>
    /// <param name="position">The position of the waypoint</param>
    /// <param name="direction">The direction the player will be facing</param>
    public void AddWayPoint(Vector2 position, PlayerDirection direction)
    {
        if(WayPoints.Count == 0)
        {
            WayPoints.Add(new WayPoint(position, direction));
            return;
        }

        int lastIndex = WayPoints.Count - 1;
        Vector2 lastPosition = WayPoints[lastIndex].Position;

        if(Vector2.Distance(lastPosition, position) > Mathf.Epsilon)
        {
            Position = position;
            Direction = direction;
            WayPoints.Add(new WayPoint(position, direction));
        }
    }

    private void InitStartingPoint()
    {
        WayPoints = new List<WayPoint>();
        if(gameObject.tag.Equals("Player"))
        {
            if(PlayerSpawn.PlayerPosition.Equals(Vector3.zero) && PlayerIndex != null)
                PlayerSpawn.PlayerPosition = PlayerIndex.position; 
            Position = PlayerSpawn.PlayerPosition;
            Direction = PlayerSpawn.PlayerDirection;
        }
    }
}