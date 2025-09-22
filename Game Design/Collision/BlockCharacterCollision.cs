using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BlockCharacterCollision is a class that is 
/// used to ignore collisions betweeen two 
/// BoxCollider2Ds.
/// </summary>
public class BlockCharacterCollision : MonoBehaviour
{
    public BoxCollider2D characterCollider;
    public BoxCollider2D characterBlockerCollider;
    
    void Start()
    {
        Physics2D.IgnoreCollision(characterCollider, characterBlockerCollider, true);
    }
}