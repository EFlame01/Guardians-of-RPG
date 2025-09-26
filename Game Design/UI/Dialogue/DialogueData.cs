// using System;
using UnityEngine;

/// <summary>
/// class that inherits from ScriptableObject class.
/// DialogueData is responsible for holding the data for
/// a specific set of dialogue that the player will then be
/// able to interact with.
/// </summary>
[CreateAssetMenu(menuName = "Dialogue/New Dialogue Container")]
public class DialogueData : ScriptableObject
{
    // [SerializeField] public string[] Actors;
    public TextAsset InkJSON;
}