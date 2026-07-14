
/// <summary>
/// BeforeRoundState is a class that extends
/// the <c>BattleState</c> class. BeforeRoundState
/// sets up the variables needed to check for items
/// and abilities that activate before the round starts.
/// 
/// Once the action is completed, it will move 
/// to the <c>OptionState</c>.
/// </summary>
public class BeforeRoundState : BattleState
{
    //private variables
    private BattleCharacter _battlePlayer;
    private BattleCharacter[] _battleAllies;
    private BattleCharacter[] _battleEnemies;
    private DialogueData _dialogueData;
    private TextBox _narrationTextBox;
    private BattleActionEffect _battleActionEffect;

    //Constructor
    public BeforeRoundState(BattleCharacter battlePlayer, BattleCharacter[] battleAllies, BattleCharacter[] battleEnemies, DialogueData dialogueData, TextBox textBox, BattleActionEffect battleActionEffect)
    {
        _battlePlayer = battlePlayer;
        _battleAllies = battleAllies;
        _battleEnemies = battleEnemies;
        _dialogueData = dialogueData;
        _narrationTextBox = textBox;
        _battleActionEffect = battleActionEffect;
    }

    public override void Enter()
    {
        CheckCharactersBeforeRound();
    }

    public override void Update()
    {
        if (_battleActionEffect.FinishedBeforeRound)
            NextState = Units.OPTION_STATE;
    }

    public override void Exit()
    {

    }

    private void CheckCharactersBeforeRound()
    {
        _battleActionEffect.SetUpBeforeRoundEffect(_battlePlayer, _battleAllies, _battleEnemies, _narrationTextBox, _dialogueData, PrevState, CurrentState);
        _battleActionEffect.StartBeforeRoundEffect();
    }
}