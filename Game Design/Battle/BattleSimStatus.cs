using System;
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
    public static bool CanRun = true;
    public static bool RunSuccessful;
    public static bool DidPlayerWin;
    public static bool GameOverScreenIfLost;
    public static GameObject BlindSymbol;
    public static GameObject BurnSymbol;
    public static GameObject CharmSymbol;
    public static GameObject ConfuseSymbol;
    public static GameObject DeafenSymbol;
    public static GameObject ExhaustionSymbol;
    public static GameObject FrozenSymbol;
    public static GameObject PetrifiedSymbol;
    public static GameObject PoisonSymbol;
    public static GameObject RestrainSymbol;
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
        if (type.Equals("ALLY"))
        {
            if (Allies.Count > 0)
                return true;
            foreach (Character c in Graveyard)
            {
                if (c.Type.Equals(type))
                    return true;
            }
            return false;
        }
        else if (type.Equals("ENEMY"))
        {
            if (Enemies.Count > 0)
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
        RunSuccessful = false;
        CanRun = true;
        DidPlayerWin = false;
        GameOverScreenIfLost = false;
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
    public static void AssignStatusGameObjects(GameObject blind, GameObject burn, GameObject charm, GameObject confuse, GameObject deafen, GameObject exhausion, GameObject frozen, GameObject petrified, GameObject poison, GameObject restrain, GameObject sleep, GameObject stun)
    {
        BlindSymbol = blind;
        BurnSymbol = burn;
        CharmSymbol = charm;
        ConfuseSymbol = confuse;
        DeafenSymbol = deafen;
        FrozenSymbol = frozen;
        PetrifiedSymbol = petrified;
        PoisonSymbol = poison;
        RestrainSymbol = restrain;
        SleepSymbol = sleep;
        StunSymbol = stun;
    }

    public static void OrderQueueForAfterRound(Queue<Character> queue)
    {
        AfterRoundStarted = true;
        queue.Clear();
        foreach (Character enemy in Enemies)
        {
            if (HasAfterRoundCondition(enemy))
                queue.Enqueue(enemy);
        }
        foreach (Character ally in Allies)
        {
            if (HasAfterRoundCondition(ally))
                queue.Enqueue(ally);
        }
        if (HasAfterRoundCondition(Player.Instance()))
            queue.Enqueue(Player.Instance());
    }

    private static bool HasAfterRoundCondition(Character c)
    {
        //TODO: add another condition for if they have ability that activates after round
        foreach (StatusCondition statusCondition in c.BattleStatus.StatusConditions.Values)
        {
            if (statusCondition.Condition.Equals("AFTER ROUND"))
                return true;
        }

        return false;
    }

    public static void CheckGraveyardStatus(Character character)
    {
        string status = "";
        int graveIndex = Graveyard.FindIndex(c => c == character);
        //Check if character is in graveyard
        //if character is in graveyard, check if they belong in graveyard
        //if character does not belong in graveyard, remove them from graveyard

        if (graveIndex != -1)
            status = "IN GRAVEYARD";
        else
            status = "NOT IN GRAVEYARD";

        switch (status)
        {
            case "IN GRAVEYARD":
                if (character.BaseStats.Hp < 0)
                    return;
                else
                {
                    if (character.Type.Equals("ALLY") || character.Type.Equals("PLAYER"))
                        Allies.Add(character);
                    if (character.Type.Equals("ENEMY") || character.Type.Equals("BOSS"))
                        Enemies.Add(character);
                }
                break;
            default:
                break;
        }
    }

    public static BattleCharacter GetBattleCharacter(Character character, BattleCharacter battlePlayer, BattleCharacter[] allies, BattleCharacter[] enemies)
    {
        List<BattleCharacter> battleCharacters = new()
        {
            battlePlayer
        };
        battleCharacters.AddRange(allies);
        battleCharacters.AddRange(enemies);

        foreach (BattleCharacter c in battleCharacters)
        {
            try
            {
                if (c.Character != null && c.Character.Equals(character))
                    return c;
            }
            catch (Exception e)
            {
                if (c == null)
                    Debug.LogWarning("BattleCharacter is null..." + e.Message);
                else if (c.Character == null)
                    Debug.LogWarning("Character object inside BattleCharacter is null..." + e.Message);
            }
        }
        return null;
    }

    public static GameObject ReturnStatusConditionSymbol(string name)
    {
        return name switch
        {
            "BURN" => BurnSymbol,
            "POISON" => PoisonSymbol,
            "STUN" => StunSymbol,
            "SLEEP" => SleepSymbol,
            "PETRIFIED" => PetrifiedSymbol,
            "FROZEN" => FrozenSymbol,
            _ => null,
        };

    }

    public static void AddToGraveYard(Character character)
    {
        RoundKnockOuts.Add(character);
        Graveyard.Add(character);
        if (character.Type.Equals("ALLY"))
            Allies.Remove(character);
        else if (character.Type.Equals("ENEMY"))
            Enemies.Remove(character);
    }

    public static bool OnSameSide(Character c1, Character c2)
    {
        if (c1.Type.Equals(c2.Type))
            return true;

        if ((c1.Type.Equals("PLAYER") || c2.Type.Equals("PLAYER")) && (c1.Type.Equals("ALLY") || c2.Type.Equals("ALLY")))
            return true;

        if ((c1.Type.Equals("BOSS") || c2.Type.Equals("BOSS")) && (c1.Type.Equals("ENEMY") || c2.Type.Equals("ENEMY")))
            return true;

        return false;
    }
}