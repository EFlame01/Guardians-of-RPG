using UnityEngine;

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

    public static bool ActionEffectStateStarted = false;

    //Constructor
    public ActionEffectState(BattleCharacter battlePlayer, BattleCharacter[] battleAllies, BattleCharacter[] battleEnemies, Camera camera, DialogueData dialogueData, TextBox textBox, BattleActionEffect battleActionEffect)
    {
        CurrentState = Units.ACTION_EFFECT_STATE;
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
        if (!ActionEffectStateStarted)
        {
            ActionEffectStateStarted = true;
            _battleActionEffect.SetUpBattleActionEffect(BattleSimStatus.ChosenCharacter, _battlePlayer, _battleAllies, _battleEnemies, _camera, _narrationTextBox, _dialogueData, PrevState, CurrentState);
            _battleActionEffect.StartActionEffect();
        }
        else
            CheckStatus();
    }

    public override void Update()
    {
        if (_battleActionEffect.StartedDialogue && DialogueManager.Instance.DialogueEnded && _battleActionEffect.FinishedAction)
        {
            _battleActionEffect.StartedDialogue = false;
            CheckStatus();
        }
        else if (_battleActionEffect.Target == null && _battleActionEffect.FinishedAction)
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
        if (BattleSimStatus.RunSuccessful)
        {
            NextState = Units.END_BATTLE;
            return;
        }

        if (_battleActionEffect.Target.BaseStats.Hp == 0)
            BattleSimStatus.AddToGraveYard(_battleActionEffect.Target);

        if (_battleActionEffect.TargetQueue.Count == 0)
        {
            if (BattleSimStatus.RoundKnockOuts.Count > 0)
            {
                //if characters are knocked out, go to knock out state
                // ActionEffectStateStarted = false;
                NextState = Units.KNOCK_OUT_STATE;
            }
            else if (HasSecondaryEffect() && !PrevState.Equals(Units.ACTION_EFFECT_STATE_2))
            {
                NextState = Units.ACTION_EFFECT_STATE_2;
            }
            else
            {
                BattleSimStatus.ChosenCharacter.BattleStatus.SetTurnStatus(TurnStatus.NOTHING);
                //go to character action state
                // ActionEffectStateStarted = false;
                NextState = Units.CHARACTER_ACTION_STATE;
            }
        }
        else
        {
            //perform the action on the next target
            _battleActionEffect.StartActionEffect();
        }
    }

    private bool HasSecondaryEffect()
    {
        Move move = BattleSimStatus.ChosenCharacter.BattleStatus.ChosenMove;
        return move != null && move.SecondaryEffects != null && move.SecondaryEffects.Length > 0;
    }

}