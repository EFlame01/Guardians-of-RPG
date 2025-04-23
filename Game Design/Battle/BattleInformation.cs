using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleInformation is the class that helps with
/// gathering the information needed to start the Battle
/// Simulator.
/// </summary>
public class BattleInformation : MonoBehaviour
{
    //public static variables
    public static BattleCharacterData BattlePlayerData;
    public static BattleCharacterData[] BattleAlliesData = new BattleCharacterData[2];
    public static BattleCharacterData[] BattleEnemiesData = new BattleCharacterData[3];
    public static string Environment;
    public static Vector3 PlayerPosition = new Vector3(0, 0, 0);
    public static string[] StoryFlagsIfWon;

    /// <summary>
    /// Resets the information in the
    /// <c>BattleInformation</c> class 
    /// to prepare for the next battle.
    /// </summary>
    public static void ResetBattleInformation()
    {
        BattlePlayerData = null;
        Array.Clear(BattleAlliesData, 0, BattleAlliesData.Length);
        Array.Clear(BattleEnemiesData, 0, BattleEnemiesData.Length);
        Environment = null;
    }
}
