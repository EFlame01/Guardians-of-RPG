using System.Collections.Generic;

///<summary>
///BattleStatus is a class that holds the
///status of a character during battle.
///</summary>
public class BattleStatus
{
    public Move ChosenMove;
    public Item ChosenItem;
    public List<Character> ChosenTargets { get; private set; }
    public Dictionary<string, StatusCondition> StatusConditions { get; private set; }
    public string ProtectionStatus { get; private set; }
    public TurnStatus TurnStatus { get; private set; }
    public string TurnStatusTag { get; private set; }
    public bool Ability { get; private set; }
    public bool ProtectBreaker { get; private set; }
    public Dictionary<string, bool> Immunities { get; private set; }
    public int RechargeTime { get; private set; }
    public bool IncludeStab { get; private set; }
    public int ExhaustionLevel { get; private set; }
    public InitiativeStatus InitiativeStatus { get; private set; }
    public BlockStatus BlockStatus { get; private set; }
    public RunStatus RunStatus { get; private set; }
    public bool SkipTurn { get; private set; }
    public bool RoundCompleted { get; private set; }

    //Constructor
    public BattleStatus()
    {
        ChosenTargets = new List<Character>();
        StatusConditions = new Dictionary<string, StatusCondition>();
        ProtectionStatus = "NONE";
        TurnStatus = TurnStatus.NOTHING;
        Immunities = new Dictionary<string, bool>
        {
            { "BLIND", false },
            { "BURN", false },
            { "CHARM", false },
            { "CONFUSE", false },
            { "DEAFEN", false },
            { "EXHAUSTION", false },
            { "FROZEN", false },
            { "PETRIFIED", false },
            { "POISON", false },
            { "RESTRAIN", false },
            { "STUN", false },
            { "SLEEP", false },
            { "STATUS_CONDITION", false }
        };
        ExhaustionLevel = 0;
        InitiativeStatus = new InitiativeStatus();
        BlockStatus = new BlockStatus();
        RunStatus = new RunStatus();
    }

    //Getters and Setters
    public void SetProtectionStatus(string protectionStatus)
    {
        ProtectionStatus = protectionStatus ?? "NOTHING";
    }
    public void SetTurnStatus(TurnStatus turnStatus)
    {
        TurnStatus = turnStatus;

    }
    public void SetAbilityUse(bool useAbility)
    {
        Ability = useAbility;
    }
    public void SetProtectBreaker(bool breakProtect)
    {
        ProtectBreaker = breakProtect;
    }
    public void SetRechargeTime(int rechargeTime)
    {
        RechargeTime = rechargeTime < 0 ? 0 : rechargeTime;
    }
    public void SetStab(bool includeStab)
    {
        IncludeStab = includeStab;
    }
    public void SetTurnStatusTag(string tag)
    {
        TurnStatusTag = tag;
    }
    public void SetExhaustionLevel()
    {
        ExhaustionLevel = ExhaustionLevel switch
        {
            Units.EXHAUSTION_LEVEL_1 => Units.EXHAUSTION_LEVEL_2,
            Units.EXHAUSTION_LEVEL_2 => Units.EXHAUSTION_LEVEL_3,
            Units.EXHAUSTION_LEVEL_3 => Units.EXHAUSTION_LEVEL_4,
            Units.EXHAUSTION_LEVEL_4 => Units.EXHAUSTION_LEVEL_5,
            Units.EXHAUSTION_LEVEL_5 => Units.EXHAUSTION_LEVEL_6,
            _ => 0,
        };

    }
    public void SetRoundCompleted(bool value)
    {
        RoundCompleted = value;
    }

    ///<summary>
    /// Resets the battle
    /// status of the <c>Character</c>, but does not
    /// remove status conditions that are
    /// BURN, POISON, or STUN from the character.
    ///</summary>
    public void ResetBattleStatus()
    {
        ChosenTargets.Clear();
        ProtectionStatus = "NONE";
        TurnStatus = TurnStatus.NOTHING;
        TurnStatusTag = null;
        StatusConditions.Remove("CHARM");
        StatusConditions.Remove("CONFUSE");
        StatusConditions.Remove("EXHAUSTION");
        StatusConditions.Remove("FLINCH");
        StatusConditions.Remove("FRIGHTEN");
        StatusConditions.Remove("FROZEN");
        StatusConditions.Remove("PETRIFIED");
        StatusConditions.Remove("RESTRAIN");
        StatusConditions.Remove("SLEEP");
        ExhaustionLevel = 0;
        InitiativeStatus = new InitiativeStatus();
        BlockStatus = new BlockStatus();
        RunStatus = new RunStatus();
    }

    /// <summary>
    /// Resets the battle status for the
    /// <c>Character</c> for the round.
    /// </summary>
    public void ResetRound()
    {
        ChosenItem = null;
        ChosenMove = null;
        ChosenItem = null;
        ChosenTargets.Clear();
        ProtectionStatus = "NONE";
        TurnStatus = TurnStatus.NOTHING;
        TurnStatusTag = null;
    }
}