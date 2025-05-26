using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WayPoint is a class that marks the position
/// and direction of a player.
/// </summary>
[Serializable]
public class WayPoint
{
    [SerializeField] public Vector2 Position;
    [SerializeField] public PlayerDirection Direction;

    //Constructor
    public WayPoint(Vector2 position, PlayerDirection diretion)
    {
        Position = position;
        Direction = diretion;
    }
}