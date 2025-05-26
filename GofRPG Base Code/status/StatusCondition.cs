using System.Collections.Generic;

///<summary>
/// StatusCondition is an abstract class that the
/// holds the method that allows all the
/// status conditions to be implemented.
///</summary>
public abstract class StatusCondition
{
    protected Dictionary<string, bool> _statusCompatabilityDictionary;
    public string Name {get; protected set;}
    public string AfflictionText {get; protected set;}
    public string WhenToImplement {get; protected set;}

    ///<summary>
    /// Implements the status condition on the
    /// <paramref name="character"/>. The effect will
    /// vary depending on the status condition.
    ///</summary>
    ///<param name="character"> the character being inflicted.</param>
    public abstract void ImplementStatusCondition(Character character);

    ///<summary>
    /// Checks if the <paramref name="statusCondition"/> can be stacked
    /// onto the <paramref name="character"/> based on the 
    /// <paramref name="character"/>'s existing status condition.
    ///</summary>
    ///<param name="character"> the character to be checked. </param>
    ///<param name="statusCondition"> the status condition that wants to be inflicted. </param>
    ///<returns> TRUE it can stack. FALSE if it cannot. </returns>
    public static bool CanStackStatusCondition(Character character, string statusCondition)
    {
        /*Status conditions that stack:
            - BLIND: BURN, CONFUSE, DEAFEN, EXHAUSTION, FLINCH, FRIGHTEN, FROZEN, PETRIFIED, POISON, RESTRAIN, SLEEP, STUN
            - BURN: BLIND, CHARM, CONFUSE, DEAFEN, EXHAUSTION, FLINCH, FRIGHTEN, FROZEN, PETRIFIED, POISON, RESTRAIN, SLEEP, STUN
            - CHARM: BLIND, BURN, CONFUSE, DEAFENM, EXHAUSTION, FLINCH, FRIGHTEN, FROZEN, PETRIFIED, POISON, RESTRAIN, SLEEP, STUN
            - CONFUSE: BLIND, BURN, DEAFEN, EXHAUSTION, FLINCH, FRIGHTEN, FROZEN, PETRIFIED, POISON, RESTRAIN, SLEEP, STUN
            - DEAFEN: BLIND, BURN, CONFUSE, EXHAUSTION, FLINCH, FRIGHTEN, FROZEN, PETRIFIED, POISON, RESTRAIN, SLEEP, STUN
            - EXHAUSTION: BLIND, BURN, CHARM, CONFUSE, DEAFEN, EXHAUSTION, FLINCH, FRIGHTEN, FROZEN, PETRIFIED, POISON, RESTRAIN, SLEEP, STUN
            - FLINCH: BLIND, BURN, CHARM, CONFUSE, DEAFEN, EXHAUSTION, FROZEN, PETRIFIED, POISON, RESTRAIN, SLEEP, STUN
            - FRIGHTEN: BLIND, BURN, CHARM, CONFUSE, DEAFEN, EXHASUTION, FLINCH, FROZEN, PETRIFIED, POISON, RESTRAIN, SLEEP, STUN
            - FROZEN: PETRIFIED, RESTRAIN
            - PETRIFIED: RESTRAIN
            - POISON: BLIND, BURN, CHARM, CONFUSE, DEAFEN, EXHAUSTION, FLINCH, FRIGHTEN, FROZEN, PETRIFIED, RESTRAIN, SLEEP, STUN
            - RESTRAIN: BLIND, BURN, CHARM, CONFUSE, DEAFEN, EXHAUSTION, FLINCH, FRIGHTEN, FROZEN, PETRIFIED, POISON, SLEEP, STUN
            - SLEEP: BURN, DEAFEN, FROZEN, PETRIFIED, POISON, RESTRAIN, STUN
            - STUN: BLIND, BURN, CHARM, CONFUSE, DEAFEN, EXHAUSTION, FLINCH, FRIGHTEN, FROZEN, PETRIFIED, POISON, RESTRAIN, SLEEP
        */
        Dictionary<string, StatusCondition> statusConditions = character.BattleStatus.StatusConditions;
        foreach(var statusInfo in statusConditions)
        {
            if(!statusInfo.Value._statusCompatabilityDictionary[statusCondition])
                return false;
        }
        
        return true;
    }

    ///<summary>
    /// Removes the <paramref name="statusCondition"/> from the 
    /// <paramref name="character"> if it exists.
    ///</summary>
    ///<param name="character"> the character having the status condition removed.</param>
    ///<param name="statusCondition"> the status condition to be removed.</param>
    public static void RemoveStatusCondition(Character character, string statusCondition)
    {
        character.BattleStatus.StatusConditions.Remove(statusCondition);
    }

    public static StatusCondition GenerateStatusCondition
    (
        string status, 
        int charmDuration, 
        int confuseDuration,
        int rollAdvantage,
        int freezeRate, 
        int poisonRate, 
        int restrainDuration,
        int sleepRate, 
        int stunRate
    )
    {
        return status switch
        {
            "BLIND" => new Blind(),
            "BURN" => new Burn(),
            "CHARM" => new Charm(charmDuration),
            "CONFUSE" => new Confuse(confuseDuration),
            "DEAFEN" => new Deafen(),
            "EXHAUSTION" => new Exhaustion(),
            "FLINCH" => new Flinch(),
            "FRIGHTEN" => new Frighten(rollAdvantage),
            "FROZEN" => new Frozen(freezeRate),
            "PETRIFIED" => new Petrified(),
            "POISON" => new Poison(poisonRate),
            "RESTRAIN" => new Restrain(restrainDuration),
            "SLEEP" => new Sleep(sleepRate),
            "STUN" => new Stun(stunRate),
            _ => null
        };
    }
}