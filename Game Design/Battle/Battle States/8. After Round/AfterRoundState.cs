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

    BattleActionEffect _battleActionEffect;

    //Constructor
    public AfterRoundState(BattleActionEffect battleActionEffect)
    {
        CurrentState = Units.AFTER_ROUND_STATE;
        _battleActionEffect = battleActionEffect;
    }

    public override void Enter()
    {
        Debug.Log("After Round State...");
        if (!BattleSimStatus.AfterRoundStarted)
            BattleSimStatus.OrderQueueForAfterRound(_battleActionEffect.TargetQueue);

        if (!RoundOver() || _battleActionEffect.TargetQueue.Count > 0)
            DisplayAfterRoundEffect();
        else
        {
            BattleSimStatus.AfterRoundStarted = false;
            if (BattleOver())
                NextState = Units.BATTLE_OVER_STATE;
            else
                NextState = Units.OPTION_STATE;
        }
    }

    public override void Update()
    {
        if (_battleActionEffect.StartedDialogue && DialogueManager.Instance.DialogueEnded && _battleActionEffect.FinishedAfterRound)
        {
            _battleActionEffect.StartedDialogue = false;
            CheckStatus();
        }
        else if (_battleActionEffect.Target == null && _battleActionEffect.FinishedAfterRound)
        {
            CheckStatus();
        }
    }

    public override void Exit()
    {
        // BattleSimStatus.AfterRoundStarted = false;
        // BattleSimStatus.AfterRoundStarted = NextState switch
        // {
        //     Units.KNOCK_OUT_STATE => true,
        //     _ => false
        // };
    }

    private void DisplayAfterRoundEffect()
    {
        TextBoxBattle.EndNarrationNow = true;
        _battleActionEffect.StartAfterRoundEffect();
    }

    private void CheckStatus()
    {
        if (_battleActionEffect.Target.BaseStats.Hp <= 0)
            BattleSimStatus.AddToGraveYard(_battleActionEffect.Target);

        if (_battleActionEffect.TargetQueue.Count <= 0)
        {

            if (BattleSimStatus.RoundKnockOuts.Count > 0)
            {
                //if characters are knocked out, go to knock out state
                BattleSimStatus.AfterRoundStarted = true;
                NextState = Units.KNOCK_OUT_STATE;
            }
            else
            {
                //go to previous state
                BattleSimStatus.AfterRoundStarted = false;
                NextState = Units.OPTION_STATE;
            }
        }
        else
        {
            Enter();
        }

    }
}
