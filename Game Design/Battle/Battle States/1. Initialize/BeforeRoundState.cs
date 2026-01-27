using UnityEngine;
using Ink.Runtime;

public class BeforeRoundState : BattleState
{
    private BattleCharacter BattlePlayer;
    private BattleCharacter[] BattleAllies;
    private BattleCharacter[] BattleEnemies;
    private DialogueData DialogueData;
    private TextBox NarrationTextBox;
    private BattleActionEffect BattleActionEffect;

    public BeforeRoundState(BattleCharacter battlePlayer, BattleCharacter[] battleAllies, BattleCharacter[] battleEnemies, DialogueData dialogueData, TextBox textBox, BattleActionEffect battleActionEffect)
    {
        BattlePlayer = battlePlayer;
        BattleAllies = battleAllies;
        BattleEnemies = battleEnemies;
        DialogueData = dialogueData;
        NarrationTextBox = textBox;
        BattleActionEffect = battleActionEffect;
    }

    public override void Enter()
    {
        CheckCharactersBeforeRound();
    }

    public override void Update()
    {
        if (BattleActionEffect.FinishedBeforeRound)
            NextState = Units.OPTION_STATE;
    }

    public override void Exit()
    {

    }

    private void CheckCharactersBeforeRound()
    {
        BattleActionEffect.SetUpBeforeRoundEffect(BattlePlayer, BattleAllies, BattleEnemies, NarrationTextBox, DialogueData, PrevState, CurrentState);
        BattleActionEffect.StartBeforeRoundEffect();
    }
}