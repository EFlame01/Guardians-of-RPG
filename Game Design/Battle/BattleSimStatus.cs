using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleSimStatus is the class that
/// holds the static variables needed
/// for many of the <c>BattleStates</c>
/// to operate.
/// </summary>
public class BattleSimStatus
{
    //public static variables
    public static List<Character> Allies = new List<Character>();
    public static List<Character> Enemies = new List<Character>();
    public static List<Character> Graveyard = new List<Character>();
    public static Queue<Character> BattleQueue = new Queue<Character>();
    public static List<Character> RoundKnockOuts = new List<Character>();
    public static bool EndPlayerOption;
    public static Character ChosenCharacter;
    public static bool UpdatedCharacterHUD;
    public static string SceneName;
    public static bool StartAfterRound = false;
    public static int TotalRounds = 0;
    public static bool RoundStarted;
    public static bool AfterRoundStarted;
    public static GameObject BurnSymbol;
    public static GameObject FrozenSymbol;
    public static GameObject PetrifiedSymbol;
    public static GameObject PoisonSymbol;
    public static GameObject SleepSymbol;
    public static GameObject StunSymbol;
    
    /// <summary>
    /// Determines if the <c>Character</c> has
    /// any other <c>Character</c>s of the same
    /// type as them in battle or in the graveyard
    /// based on the <paramref name="type"/>.
    /// 
    /// This is helpful when selecting targets to perform
    /// moves on.
    /// </summary>
    /// <param name="type">the type of <c>Character</c> the character is</param>
    /// <returns></returns>
    public static bool CharacterTypePresent(string type)
    {
        if(type.Equals("ALLY"))
        {
            if(Allies.Count > 0)
                return true;
            foreach(Character c in Graveyard)
            {
                if(c.Type.Equals(type))
                    return true;
            }
            return false;
        }
        else if(type.Equals("ENEMY"))
        {
            if(Enemies.Count > 0)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    /// <summary>
    /// Clears the BattleStatus for the <c>Player</c> and the
    /// <c>BattleSimulator</c>.
    /// </summary>
    public static void ClearBattleSimStatus()
    {
        Player.Instance().BattleStatus.ResetBattleStatus();
        Allies.Clear();
        Enemies.Clear();
        Graveyard.Clear();
        BattleQueue.Clear();
        RoundKnockOuts.Clear();
        ChosenCharacter = null;
        SceneName = null;
        EndPlayerOption = true;
        TotalRounds = 0;
        StartAfterRound = false;
        AfterRoundStarted = false;
    }

    /// <summary>
    /// Assigns the Status Condition GameObjects
    /// to variables inside <c>BattleSimStatus</c>
    /// in order to instantiate them later.
    /// </summary>
    /// <param name="burn">BurnSymbol gameObject</param>
    /// <param name="frozen">FrozenSymbol gameObject</param>
    /// <param name="petrified">PetrifiedSymbol gameObject</param>
    /// <param name="poison">PoisonSymbol gameObject</param>
    /// <param name="sleep">SleepSymbol gameObject</param>
    /// <param name="stun">StunSumbol gameObject</param>
    public static void AssignStatusGameObjects(GameObject burn, GameObject frozen, GameObject petrified, GameObject poison, GameObject sleep, GameObject stun)
    {
        BurnSymbol = burn;
        FrozenSymbol = frozen;
        PetrifiedSymbol = petrified;
        PoisonSymbol = poison;
        SleepSymbol = sleep;
        StunSymbol = stun;
    }

    public static void OrderQueueForAfterRound()
    {
        AfterRoundStarted = true;
        BattleQueue.Clear();
        foreach(Character enemy in Enemies)
            BattleQueue.Enqueue(enemy);
        foreach(Character ally in Allies)
            BattleQueue.Enqueue(ally);
        BattleQueue.Enqueue(Player.Instance());
    }
}