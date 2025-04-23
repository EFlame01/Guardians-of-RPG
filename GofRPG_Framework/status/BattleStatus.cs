using System.Collections;
using System.Collections.Generic;

///<summary>
///BattleStatus is a class that holds the
///status of a character during battle.
///</summary>
public class BattleStatus
{
    public Move ChosenMove;
    public Item ChosenItem;
    public List<Character> ChosenTargets {get; private set;}
    public Dictionary<string, StatusCondition> StatusConditions {get; private set;}
    public string ProtectionStatus {get; private set;}
    public TurnStatus TurnStatus {get; private set;}
    public string TurnStatusTag {get; private set;}

    public bool Ability {get; private set;}
    public bool ProtectBreaker {get; private set;}
    public Dictionary<string, bool> Immunities {get; private set;}
    public int RechargeTime {get; private set;}
    public bool IncludeStab {get; private set;}
    public int ExhaustionLevel {get; private set;}
    public double RollAdvantage {get; private set;}
    public bool CanEscape {get; private set;}
    public bool RollInitiative {get; private set;}
    public int InitiativeCount {get; private set;}
    public bool RollRun {get; private set;}
    public bool SkipTurn {get; private set;}
    public bool RoundCompleted {get; private set;}
    private int _rollAdvStage = 0;

    //Constructor
    public BattleStatus()
    {
        ChosenTargets = new List<Character>();
        StatusConditions = new Dictionary<string, StatusCondition>();
        ProtectionStatus = "NONE";
        TurnStatus = TurnStatus.NOTHING;
        Immunities = new Dictionary<string, bool>
        {
            { "BURN", false },
            { "POISON", false },
            { "STUN", false },
            { "SLEEP", false },
            { "FROZEN", false },
            { "PETRIFIED", false },
            { "STATUS_CONDITION", false }
        };
        ExhaustionLevel = 0;
        RollAdvantage = Units.STAGE_0;
        CanEscape = true;
        InitiativeCount = 3;
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
        RechargeTime = rechargeTime  < 0 ? 0 : rechargeTime;
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
    public void SetRollAdvantage(int stage)
    {
        if(_rollAdvStage + stage > 6)
            RollAdvantage = Units.STAGE_POS_6;
        else if(_rollAdvStage + stage < 6)
            RollAdvantage = Units.STAGE_NEG_6;
        else
        {
            RollAdvantage = _rollAdvStage + stage switch
            {
                6 => Units.STAGE_POS_6,
                5 => Units.STAGE_POS_5,
                4 => Units.STAGE_POS_4,
                3 => Units.STAGE_POS_3,
                2 => Units.STAGE_POS_2,
                1 => Units.STAGE_POS_1,
                -6 => Units.STAGE_POS_6,
                -5 => Units.STAGE_NEG_5,
                -4 => Units.STAGE_NEG_4,
                -3 => Units.STAGE_NEG_3,
                -2 => Units.STAGE_NEG_2,
                -1 => Units.STAGE_NEG_1,
                _ => Units.STAGE_0,
            };
        }
    }
    public void SetCanEscape(bool canEscape)
    {
        CanEscape = canEscape;
    }

    public bool CanRollInitiative()
    {
        return InitiativeCount > 0;
    }
    
    public void SetRollInitiativeTrue()
    {
        if(!CanRollInitiative())
            return;
        
        RollInitiative = true;
        // InitiativeCount--;
    }

    public void SetRollInitiativeFalse()
    {
        RollInitiative = false;
    }

    public void SetRollRun(bool value)
    {
        RollRun = value;
    }

    public void SetRoundCompleted(bool value)
    {
        RoundCompleted = value;
    }

    ///<summary>
    /// Resets the battle
    /// status of the player, but does not
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
        CanEscape = true;
        SetRollAdvantage(0);
    }

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