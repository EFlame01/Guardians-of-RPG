using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffectState2 : BattleState
{
    //private variables
    private Camera _camera;
    private TextBox _narrationTextBox;
    private DialogueData _dialogueData;
    private BattleCharacter _battlePlayer;
    private BattleCharacter[] _battleAllies;
    private BattleCharacter[] _battleEnemies;
    private BattleActionEffect _battleActionEffect;

    //Constructor
    public ActionEffectState2(BattleCharacter battlePlayer, BattleCharacter[] battleAllies, BattleCharacter[] battleEnemies, Camera camera, DialogueData dialogueData, TextBox textBox, BattleActionEffect battleActionEffect)
    {
        CurrentState = Units.ACTION_EFFECT_STATE_2;
        _battlePlayer = battlePlayer;
        _battleAllies = battleAllies;
        _battleEnemies = battleEnemies;
        _camera = camera;
        _dialogueData = dialogueData;
        _narrationTextBox = textBox;
        _battleActionEffect = battleActionEffect;
    }

    public override void Enter()
    {
        _battleActionEffect.SetUpSecondaryActionEffect(BattleSimStatus.ChosenCharacter, _battlePlayer, _battleAllies, _battleEnemies, _camera, _narrationTextBox, _dialogueData, PrevState, CurrentState);
        _battleActionEffect.StartSecondaryEffect();

    }

    public override void Update()
    {
        if (_battleActionEffect.StartedDialogue && DialogueManager.Instance.DialogueEnded && _battleActionEffect.DoneWithSecondaryEffects)
        {
            CheckStatus();
        }
        else if (!_battleActionEffect.StartedDialogue && _battleActionEffect.DoneWithSecondaryEffects)
        {
            CheckStatus();
        }
    }

    public override void Exit()
    {

    }

    private void CheckStatus()
    {
        Debug.Log("Checking Status...");
        // TextBoxBattle.KeepTextBoxOpened = false;
        // TextBoxBattle.EndNarrationNow = true;

        if (_battleActionEffect.Target != null && _battleActionEffect.Target.BaseStats.Hp == 0)
            BattleSimStatus.AddToGraveYard(_battleActionEffect.Target);

        if (_battleActionEffect.TargetQueue.Count == 0)
        {
            if (BattleSimStatus.RoundKnockOuts.Count > 0)
            {
                //if characters are knocked out, go to knock out state
                NextState = Units.KNOCK_OUT_STATE;
            }
            else
            {
                BattleSimStatus.ChosenCharacter.BattleStatus.SetTurnStatus(TurnStatus.NOTHING);
                //go to previous state
                NextState = PrevState;
            }
        }
        else
            Debug.Log("We are stuck here...");
    }
}
