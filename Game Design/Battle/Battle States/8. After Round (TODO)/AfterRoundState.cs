using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AfterRoundState will take the characters
/// that survived the round and update any 
/// battle status effects for the next round.
/// 
/// After this it will determine if there
/// </summary>
public class AfterRoundState : BattleState
{

    //Constructor
    public AfterRoundState()
    {
        CurrentState = Units.AFTER_ROUND_STATE;
    }

    public override void Enter()
    {
        if(!BattleSimStatus.AfterRoundStarted)
            BattleSimStatus.OrderQueueForAfterRound();
        if(!RoundOver())
            DisplayAfterRoundEffect();
        else
        {
            BattleSimStatus.AfterRoundStarted = false;
            if(BattleOver())
                NextState = Units.BATTLE_OVER_STATE;
            else
                NextState = Units.OPTION_STATE;
        }
    }

    public override void Update()
    {
        if(_roundOver)
            return;
        if(CharacterHasBurnOrPoison(BattleSimStatus.ChosenCharacter))
            NextState = Units.ACTION_EFFECT_STATE;
        else
            NextState = Units.AFTER_ROUND_STATE;
        
    }

    public override void Exit()
    {
        
    }

    private bool CharacterHasBurnOrPoison(Character character)
    {
        Dictionary <string, StatusCondition> statusConditions = character.BattleStatus.StatusConditions;

        if(statusConditions.ContainsKey("BURN") && statusConditions["BURN"] != null)
            return true;
        else if(statusConditions.ContainsKey("POISON") && statusConditions["POISON"] != null)
            return true;
        
        return false;
    }

    private void DisplayAfterRoundEffect()
    {
        Character character = BattleSimStatus.BattleQueue.Dequeue();
        BattleSimStatus.ChosenCharacter = character;
    }
}
