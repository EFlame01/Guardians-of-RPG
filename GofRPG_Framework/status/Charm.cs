using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Charm is a class that extends the StatusCondition class. 
/// Charm will prevent the player from being able to inflict 
/// damage on the enemy side.
///</summary>
public class Charm : StatusCondition
{
    private int _chanceOfCharm = Units.CHARM_CHANCE;
    private int _roundsLeft;

    //Constructor
    public Charm(int charmDuration)
    {
        Name = "CHARM";
        AfflictionText = "charmed";
        WhenToImplement = "'DURING ROUND'";
        _roundsLeft = charmDuration;
        _statusCompatabilityDictionary = new Dictionary<string, bool>()
        {
            {"BLIND", true},
            {"BURN", true},
            {"CHARM", false},
            {"CONFUSE", true},
            {"DEAFEN", true},
            {"EXHAUSTION", true},
            {"FLINCH", true},
            {"FRIGHTEN", true},
            {"FROZEN", true},
            {"PETRIFIED", true},
            {"POISON", true},
            {"RESTRAIN", true},
            {"SLEEP", true},
            {"STUN", true},
        };
    }

    public override void ImplementStatusCondition(Character character)
    {
        if(!character.BattleStatus.TurnStatus.Equals("FIGHTING"))
            return;

        if(character.BattleStatus.ChosenMove == null)
            return;
        
        switch(character.BattleStatus.ChosenMove.Target)
        {
            case MoveTarget.ENEMY:
            case MoveTarget.ALL_ENEMIES:
                if(character.Type.Equals("ALLY") || character.Type.Equals("PLAYER"))
                    DetermineIfCharmed(character);
                break;
            case MoveTarget.USER:
            case MoveTarget.ALLY:
            case MoveTarget.ALL_ALLIES:
                if(character.Type.Equals("ENEMY"))
                    DetermineIfCharmed(character);
                break;
            case MoveTarget.EVERYONE:
                _chanceOfCharm = Units.LOWER_CHARM_CHANCE;
                DetermineIfCharmed(character);
                break;
            default:
                break;
        }
    }

    private void DetermineIfCharmed(Character character)
    {
        if(_roundsLeft <= 0)
        {
            RemoveStatusCondition(character, Name);
            return;
        }

        if(_chanceOfCharm > Random.Range(0, 100))
        {   
            character.BattleStatus.SetTurnStatus(TurnStatus.CANNOT_MOVE);
            character.BattleStatus.SetTurnStatusTag(character.Name + " is charmed by the enemy!");
        }

        _chanceOfCharm = Units.CHARM_CHANCE;
        _roundsLeft--;
    }
}