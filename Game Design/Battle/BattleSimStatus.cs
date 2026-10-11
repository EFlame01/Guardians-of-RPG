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
    public static List<Character> Allies = new();
    public static List<Character> Enemies = new();
    public static List<Character> Graveyard = new();
    public static Queue<Character> BattleQueue = new();
    public static List<Character> RoundKnockOuts = new();
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
    public static void AssignStatusGameObjects(GameObject blind, GameObject burn, GameObject charm, GameObject confuse, GameObject deafen, GameObject exhaustion, GameObject frozen, GameObject petrified, GameObject poison, GameObject restrain, GameObject sleep, GameObject stun)
    {
        BlindSymbol = blind;
        BurnSymbol = burn;
        CharmSymbol = charm;
        ConfuseSymbol = confuse;
        DeafenSymbol = deafen;
        ExhaustionSymbol = exhaustion;
        FrozenSymbol = frozen;
        PetrifiedSymbol = petrified;
        PoisonSymbol = poison;
        RestrainSymbol = restrain;
        SleepSymbol = sleep;
        StunSymbol = stun;
    }

    /// <summary>
    /// Determines the order for who goes first during
    /// the AfterRoundState and puts them inside of a queue. 
    /// The order is typically as follows:
    ///     <list type="bullet">
    ///         <item>Enemies</item>
    ///         <item>Allies</item>
    ///         <item>Player</item>
    ///     </list>
    /// </summary>
    /// <param name="queue">The queue that will have the ordered list of characters to go first during the AfterRoundState</param>
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

    /// <summary>
    /// Checks if a character has a status condition
    /// that should be implemented during the AFTER ROUND
    /// state.
    /// </summary>
    /// <param name="character">The character that will be checked for status conditions.</param>
    /// <returns><c>TRUE</c> if the character has a status condition that takes effect during the AFTER ROUND state. <c>FALSE</c> if otherwise</returns>
    private static bool HasAfterRoundCondition(Character character)
    {
        //TODO: add another condition for if they have ability that activates after round
        foreach (StatusCondition statusCondition in character.BattleStatus.StatusConditions.Values)
        {
            if (statusCondition.Condition.Equals("AFTER ROUND"))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if a <paramref name="character"/> in the <c>Graveyard</c> list and
    /// if they belong there. If they do not belong there, the character
    /// will be removed from the Graveyard and may be placed in the
    /// <c>Allies</c> or <c>Enemies</c> list depending on what type of
    /// character they are.
    /// </summary>
    /// <param name="character">The character that will be checked to see if they are in the graveyard or not.</param>
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
                    Graveyard.Remove(character);
                    if (character.Type.Equals("ALLY"))
                        Allies.Add(character);
                    if (character.Type.Equals("ENEMY") || character.Type.Equals("BOSS"))
                        Enemies.Add(character);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Retreives the battle character from the list of options based on
    /// the <paramref name="character"/>.
    /// </summary>
    /// <param name="character">The character of the BattleCharacter object in question</param>
    /// <param name="battlePlayer">The BattleCharacter of the player</param>
    /// <param name="allies">The list of BattleCharacters of the player's allies</param>
    /// <param name="enemies">The list of BattleCharacters fo the player's enemies</param>
    /// <returns>The <c>BattleCharacter</c> in question. If the character cannot be found, it will return null.</returns>
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

    /// <summary>
    /// Retrieves the status symbol <c>GameObject</c> based on the
    /// <paramref name="name"/> of the status condition.
    /// </summary>
    /// <param name="name">The name of the status symbol</param>
    /// <returns>The <c>GameObject</c> of the status symbol. If it cannot be found, it will return null.</returns>
    public static GameObject ReturnStatusConditionSymbol(string name)
    {
        return name switch
        {
            "BLIND" => BlindSymbol,
            "BURN" => BurnSymbol,
            "CHARM" => CharmSymbol,
            "CONFUSE" => ConfuseSymbol,
            "DEAFEN" => DeafenSymbol,
            "EXHAUSTION" => ExhaustionSymbol,
            "FROZEN" => FrozenSymbol,
            "PETRIFIED" => PetrifiedSymbol,
            "POISON" => PoisonSymbol,
            "RESTRAIN" => RestrainSymbol,
            "SLEEP" => SleepSymbol,
            "STUN" => StunSymbol,
            _ => null,
        };
    }

    /// <summary>
    /// Adds a <paramref name="character"/> to the <c>Graveyard</c> list
    /// and either removes the character from the <c>Allies</c> or <c>Enemies</c> 
    /// list depending on what type of character they are.
    /// </summary>
    /// <param name="character">The character being sent to the <c>Graveyard</c></param>
    public static void AddToGraveYard(Character character)
    {
        RoundKnockOuts.Add(character);
        Graveyard.Add(character);
        if (character.Type.Equals("ALLY"))
            Allies.Remove(character);
        else if (character.Type.Equals("ENEMY"))
            Enemies.Remove(character);
    }

    /// <summary>
    /// Checks if two characters are on the same team. This means
    ///     <list type="bullet">
    ///         <item>PLAYER and ALLY</item>
    ///         <item>ALLY and ALLY</item>
    ///         <item>BOSS and ENEMY</item>
    ///         <item>ENEMY and ENEMY</item>
    ///     </list>
    /// </summary>
    /// <param name="c1">character 1</param>
    /// <param name="c2">character 2</param>
    /// <returns><c>TRUE</c> if the characters are on the same side. <c>FALSE</c> if they are not.</returns>
    public static bool OnSameSide(Character c1, Character c2)
    {
        if (c1 == null || c2 == null)
            return false;

        if (c1.Type.Equals(c2.Type))
            return true;

        if ((c1.Type.Equals("PLAYER") || c2.Type.Equals("PLAYER")) && (c1.Type.Equals("ALLY") || c2.Type.Equals("ALLY")))
            return true;

        if ((c1.Type.Equals("BOSS") || c2.Type.Equals("BOSS")) && (c1.Type.Equals("ENEMY") || c2.Type.Equals("ENEMY")))
            return true;

        return false;
    }

    /// <summary>
    /// Resets the option that the <c>Player</c>
    /// chose in the previous round. This method is
    /// used to get them ready for the OPTION STATE.
    /// </summary>
    public static void ResetOptionForPlayer()
    {
        Player player = Player.Instance();

        player.BattleStatus.ChosenTargets.Clear();
        player.BattleStatus.ChosenItem = null;
        player.BattleStatus.ChosenMove = null;
        player.BattleStatus.RunStatus.SetRollRun(false);
        player.BattleStatus.RunStatus.SetRollValue(0);
        player.BattleStatus.InitiativeStatus.SetRollInitiative(false);
        player.BattleStatus.InitiativeStatus.SetInitiativeRollValue(0);
        player.BattleStatus.BlockStatus.SetRollBlock(false);
        player.BattleStatus.BlockStatus.SetBlockValue(0);
    }
}