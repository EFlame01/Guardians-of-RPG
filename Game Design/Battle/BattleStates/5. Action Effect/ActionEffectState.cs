using System.Collections;
using UnityEngine;
using Ink.Runtime;

/// <summary>
/// ActionEffectState is a class that extends the 
/// BattleState class. ActionEffectState shows the
/// effects of the action that the character did
/// to all of the targets of said action.
/// 
/// Once the action is completed, it will determine
/// if the next state should be the <c>KnockoutState</c>
/// or the <c>CharacterActionState</c>.
/// </summary>
public class ActionEffectState : BattleState
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
    public ActionEffectState(BattleCharacter battlePlayer, BattleCharacter[] battleAllies, BattleCharacter[] battleEnemies, Camera camera, DialogueData dialogueData, TextBox textBox, BattleActionEffect battleActionEffect)
    {
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
        _battleActionEffect.SetUpBattleActionEffect(BattleSimStatus.ChosenCharacter, _battlePlayer, _battleAllies, _battleEnemies, _camera, _narrationTextBox, _dialogueData);
        _battleActionEffect.StartActionEffect();
    }

    public override void Update()
    {
        if(_battleActionEffect.StartedDialogue && DialogueManager.Instance.DialogueEnded)
        {
            _battleActionEffect.StartedDialogue = false;
            CheckStatus();
        }
        else if(_battleActionEffect.Target == null && _battleActionEffect.FinishedAction)
        {
            BattleSimStatus.ChosenCharacter.BattleStatus.SetTurnStatus(TurnStatus.NOTHING);
            CheckStatus();
        }
    }

    public override void Exit()
    {

    }

    private void CheckStatus()
    {
        if(_battleActionEffect.Target.BaseStats.Hp == 0)
        {
            BattleSimStatus.RoundKnockOuts.Add(_battleActionEffect.Target);
            BattleSimStatus.Graveyard.Add(_battleActionEffect.Target);
            if(_battleActionEffect.Target.Type.Equals("ALLY"))
                BattleSimStatus.Allies.Remove(_battleActionEffect.Target);
            else if(_battleActionEffect.Target.Type.Equals("ENEMY"))
                BattleSimStatus.Enemies.Remove(_battleActionEffect.Target);
        }
        if(_battleActionEffect.TargetQueue.Count == 0)
        {
            if(BattleSimStatus.RoundKnockOuts.Count > 0)
            {
                //if characters are knocked out, go to knock out state
                NextState = "KNOCK OUT STATE";
            }
            else
            {
                BattleSimStatus.ChosenCharacter.BattleStatus.SetTurnStatus(TurnStatus.NOTHING);
                //go to character action state
                NextState = "CHARACTER ACTION STATE";
            }
        }
        else
        {
            //perform the action on the next target
            _battleActionEffect.StartActionEffect();
        }
    }

}