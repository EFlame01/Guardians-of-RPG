using UnityEngine;
using Ink.Runtime;

/// <summary>
/// InitializeState is a class that extends
/// the <c>BattleState</c> class. This class 
/// sets up the camera, <c>BattleCharacter</c>, and 
/// <c>EnvironmentDetail</c> to start the battle.
/// 
/// Once the action is completed, it will move 
/// to the <c>BeforeRoundState</c>.
/// </summary>
public class InitializeState : BattleState, IDialogue
{
    //private variables
    private string _initializeText;
    private BattleCharacter _battlePlayer;
    private BattleCharacter[] _battleAllies;
    private BattleCharacter[] _battleEnemies;
    private EnvironmentDetail[] _environmentDetails;
    private Camera _camera;
    private TextBox _narrationTextBox;
    private DialogueData _dialogueData;
    private int _numberOfCharacters;
    private bool _startedDialogue;


    //Constructor
    public InitializeState(BattleCharacter battlePlayer, BattleCharacter[] battleAllies, BattleCharacter[] battleEnemies, EnvironmentDetail[] environmentDetails, Camera camera, DialogueData dialogueData, TextBox textBox)
    {
        CurrentState = Units.INITIALIZE_STATE;
        _battlePlayer = battlePlayer;
        _battleAllies = battleAllies;
        _battleEnemies = battleEnemies;
        _environmentDetails = environmentDetails;
        _camera = camera;
        _dialogueData = dialogueData;
        _narrationTextBox = textBox;
    }

    public override void Enter()
    {
        InitializeBattleSimStatus();
    }

    public override void Update()
    {
        if (_startedDialogue && DialogueManager.Instance.DialogueEnded)
            NextState = Units.BEFORE_ROUND_STATE;

        if (!_startedDialogue && GameManager.Instance.PlayerState.Equals(PlayerState.NOT_MOVING))
            StartDialogue();
    }

    public override void Exit()
    {
        BattleSimStatus.EndPlayerOption = false;
    }

    private void InitializeBattleSimStatus()
    {
        SetUpBattleCharacter(_battlePlayer, BattleInformation.BattlePlayerData);
        for (int i = 0; i < BattleInformation.BattleAlliesData.Length; i++)
            SetUpBattleCharacter(_battleAllies[i], BattleInformation.BattleAlliesData[i]);
        for (int i = 0; i < BattleInformation.BattleEnemiesData.Length; i++)
            SetUpBattleCharacter(_battleEnemies[i], BattleInformation.BattleEnemiesData[i]);
        SetUpCameraSize();
        SetUpEnvironment();
        SetUpText();

    }

    private void SetUpBattleCharacter(BattleCharacter battleCharacter, BattleCharacterData battleCharacterData)
    {
        if (battleCharacterData != null && battleCharacterData.CharacterData != null)
        {
            _numberOfCharacters++;

            if (battleCharacterData.IsPlayer)
            {
                battleCharacter.Character = Player.Instance();
                battleCharacter.AnimationPosition = battleCharacterData.GetPlayerAnimationPosition();
            }
            else
            {
                battleCharacter.Character = CharacterMaker.Instance.GetCharacterBasedOnName(battleCharacterData.CharacterData);
                battleCharacter.AnimationPosition = battleCharacterData.CharacterAnimationPosition;
            }

            if (battleCharacter.Character != null)
            {
                battleCharacter.RuntimeAnimatorController = battleCharacterData.CharacterAnimator;
                battleCharacter.InitializeBattleCharacter();
            }
            else
                return;

            if (battleCharacter.Character.Type.Equals("ALLY"))
                BattleSimStatus.Allies.Add(battleCharacter.Character);
            else if (battleCharacter.Character.Type.Equals("ENEMY"))
                BattleSimStatus.Enemies.Add(battleCharacter.Character);

            foreach (StatusCondition sc in battleCharacter.Character.BattleStatus.StatusConditions.Values)
            {
                if (sc != null)
                    battleCharacter.CharacterHUD.AddStatusSymbol(BattleSimStatus.ReturnStatusConditionSymbol(sc.Name));
            }

        }
        else
            battleCharacter.gameObject.SetActive(false);
    }

    private void SetUpCameraSize()
    {
        if (_numberOfCharacters == 2)
            _camera.orthographicSize = 3;
        else if (_numberOfCharacters > 2 && _numberOfCharacters < 5)
            _camera.orthographicSize = 4;
        else
            _camera.orthographicSize = 5;
    }

    private void SetUpEnvironment()
    {
        foreach (EnvironmentDetail environmentDetail in _environmentDetails)
        {
            if (!environmentDetail.ID.Equals(BattleInformation.Environment))
                environmentDetail.Environment.SetActive(false);
            else
                environmentDetail.Environment.SetActive(true);
        }
    }

    private void SetUpText()
    {
        _initializeText = Player.Instance().Name;
        int numAllies = 0;
        int numEnemies = 0;

        foreach (BattleCharacter ally in _battleAllies)
        {
            if (ally.gameObject.activeSelf)
                numAllies++;
        }
        foreach (BattleCharacter enemy in _battleEnemies)
        {
            if (enemy.gameObject.activeSelf)
                numEnemies++;
        }

        //texts for if there are 0, 1, or 2 allies
        if (numAllies == 1)
            _initializeText += " and " + _battleAllies[0].Character.Name + " are fighting ";
        else if (numAllies == 2)
            _initializeText += ", " + _battleAllies[0].Character.Name + ", and " + _battleAllies[1].Character.Name + " are fighting ";
        else
            _initializeText += " is fighting ";

        //texts for if there are 1, 2, or 3 enemies
        if (numEnemies == 1)
            _initializeText += _battleEnemies[0].Character.Name + "!";
        else if (numEnemies == 2)
            _initializeText += _battleEnemies[0].Character.Name + " and " + _battleEnemies[1].Character.Name + "!";
        else if (numEnemies == 3)
            _initializeText += _battleEnemies[0].Character.Name + ", " + _battleEnemies[1].Character.Name + ", and " + _battleEnemies[2].Character.Name + "!";

        _initializeText += "!";
        _initializeText = _initializeText.Replace("Wild", "a wild");
    }

    public void StartDialogue()
    {
        _startedDialogue = true;
        TextBoxBattle.KeepTextBoxOpened = true;
        TextBoxBattle.EndNarrationNow = false;
        DialogueManager.Instance.CurrentStory = new Story(_dialogueData.InkJSON.text);
        if (DialogueManager.Instance.SetVariableState("text", _initializeText, "string"))
        {
            DialogueManager.Instance.TextBox = _narrationTextBox;
            DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
        }
        Debug.LogError($"There was an error that occurred when setting the variable state:\n- variable state: 'text'\n- value: {_initializeText}");
    }
}