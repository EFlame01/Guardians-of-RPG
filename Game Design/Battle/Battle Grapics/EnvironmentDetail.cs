using System;
using UnityEngine;

/// <summary>
/// EnvironmentDetail is a serialzied
/// class that holds the ID and background
/// for the actual environment that the
/// <c>BattleSimulator</c> will use.
/// </summary>
[Serializable]
public class EnvironmentDetail
{
    //public variables
    public string ID;
    public GameObject Environment;
}