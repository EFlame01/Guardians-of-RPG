using UnityEngine;

/// <summary>
/// PlayerSpawn is a class that keeps/sets the information
/// of the player's position during the game.
/// </summary>
public class PlayerSpawn : MonoBehaviour
{
    //public static variables
    public static PlayerDirection PlayerDirection = PlayerDirection.DOWN;
    public static Vector3 PlayerPosition = new Vector3(0, 0, 0);

    public void Awake()
    {
        if (PlayerDirection.Equals(PlayerDirection.NONE))
            PlayerDirection = PlayerDirection.DOWN;

        if (!PlayerPosition.Equals(new Vector3(0, 0, 0)))
            transform.position = PlayerPosition;
    }
}