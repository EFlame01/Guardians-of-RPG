using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerState is an enum used to
/// determine the state the user is in.
/// </summary>
public enum PlayerState
{
    MOVING,
    NOT_MOVING,
    PAUSED,
    CUT_SCENE,
    TRANSITION,
    INTERACTING_WITH_OBJECT,
    CANNOT_MOVE
}