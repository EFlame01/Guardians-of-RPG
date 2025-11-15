using UnityEngine;
using Ink.Runtime;

/// <summary>
/// InitializeState is a class that extends
/// the <c>BattleState</c> class. This class 
/// sets up the camera, <c>BattleCharacter</c>, and 
/// <c>EnvironmentDetail</c> to start the battle.
/// 
/// Once the action is completed, it will move 
/// to the <c>OptionState</c>.
/// </summary>
public class InitializeState : BattleState, IDialogue
{
    //private variables
    private string InitializeText;
    private BattleCharacter BattlePlayer;
    private BattleCharacter[] BattleAllies;
    private BattleCharacter[] BattleEnemies;
    private EnvironmentDetail[] EnvironmentDetails;
    private Camera Camera;
    private TextBox NarrationTextBox;
    private DialogueData DialogueData;
    private int numberOfCharacters;
    private bool startedDialogue;


    //Constructor
    public InitializeState(BattleCharacter battlePlayer, BattleCharacter[] battleAllies, BattleCharacter[] battleEnemies, EnvironmentDetail[] environmentDetails, Camera camera, DialogueData dialogueData, TextBox textBox)
    {
        CurrentState = Units.INITIALIZE_STATE;
        BattlePlayer = battlePlayer;
        BattleAllies = battleAllies;
        BattleEnemies = battleEnemies;
        EnvironmentDetails = environmentDetails;
        Camera = camera;
        DialogueData = dialogueData;
        NarrationTextBox = textBox;
    }

    public override void Enter()
    {
        InitializeBattleSimStatus();
    }

    public override void Update()
    {
        if (GameManager.Instance.PlayerState.Equals(PlayerState.NOT_MOVING) && InitializeText != null && !startedDialogue)
        {
            startedDialogue = true;
            StartDialogue();
        }
        if (startedDialogue && DialogueManager.Instance.DialogueEnded)
            NextState = Units.OPTION_STATE;
    }

    public override void Exit()
    {
        BattleSimStatus.EndPlayerOption = false;
    }

    private void InitializeBattleSimStatus()
    {
        SetUpBattleCharacter(BattlePlayer, BattleInformation.BattlePlayerData);

        for (int i = 0; i < BattleInformation.BattleAlliesData.Length; i++)
            SetUpBattleCharacter(BattleAllies[i], BattleInformation.BattleAlliesData[i]);

        for (int i = 0; i < BattleInformation.BattleEnemiesData.Length; i++)
            SetUpBattleCharacter(BattleEnemies[i], BattleInformation.BattleEnemiesData[i]);

        SetUpCameraSize();
        SetUpEnvironment();
        SetUpText();
    }

    private void SetUpBattleCharacter(BattleCharacter battleCharacter, BattleCharacterData battleCharacterData)
    {
        if (battleCharacterData == null)
        {
            battleCharacter.gameObject.SetActive(false);
            return;
        }

        numberOfCharacters++;

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

        if (battleCharacter.Character.Type.Equals("ALLY"))
            BattleSimStatus.Allies.Add(battleCharacter.Character);
        else if (battleCharacter.Character.Type.Equals("ENEMY"))
            BattleSimStatus.Enemies.Add(battleCharacter.Character);

        battleCharacter.RuntimeAnimatorController = battleCharacterData.CharacterAnimator;
        battleCharacter.InitializeBattleCharacter();
    }

    private void SetUpCameraSize()
    {
        if (numberOfCharacters == 2)
            Camera.orthographicSize = 3;
        else if (numberOfCharacters > 2 && numberOfCharacters < 5)
            Camera.orthographicSize = 4;
        else
            Camera.orthographicSize = 5;
    }

    private void SetUpEnvironment()
    {
        foreach (EnvironmentDetail environmentDetail in EnvironmentDetails)
        {
            if (!environmentDetail.ID.Equals(BattleInformation.Environment))
                environmentDetail.Environment.SetActive(false);
            else
                environmentDetail.Environment.SetActive(true);
        }
    }

    private void SetUpText()
    {
        InitializeText = Player.Instance().Name;
        int numAllies = 0;
        int numEnemies = 0;
        foreach (BattleCharacter ally in BattleAllies)
        {
            if (ally.gameObject.activeSelf)
                numAllies++;
        }
        foreach (BattleCharacter enemy in BattleEnemies)
        {
            if (enemy.gameObject.activeSelf)
                numEnemies++;
        }

        //if there is 1 ally
        //if there are 2 allies
        //if there are no allies
        if (numAllies == 1)
            InitializeText += " and " + BattleAllies[0].Character.Name + " are fighting ";
        else if (numAllies == 2)
            InitializeText += ", " + BattleAllies[0].Character.Name + ", and " + BattleAllies[1].Character.Name + " are fighting ";
        else
            InitializeText += " is fighting ";

        //if there is 1 enemy
        //if there are 2 enemies
        //if there are 3 enemies
        if (numEnemies == 1)
            InitializeText += BattleEnemies[0].Character.Name + "!";
        else if (numEnemies == 2)
            InitializeText += BattleEnemies[0].Character.Name + ", and " + BattleEnemies[1].Character.Name + "!";
        else if (numEnemies == 3)
            InitializeText += BattleEnemies[0].Character.Name + ", " + BattleEnemies[1].Character.Name + ", and " + BattleEnemies[2].Character.Name + "!";

        InitializeText += "!";

        InitializeText = InitializeText.Replace("Wild", "a wild");
    }

    public void StartDialogue()
    {
        DialogueManager.Instance.CurrentStory = new Story(DialogueData.InkJSON.text);
        DialogueManager.Instance.CurrentStory.variablesState["text"] = InitializeText;
        // NarrationTextBox.OpenTextBox();
        // NarrationTextBox.StartNarration(DialogueData);
        DialogueManager.Instance.TextBox = NarrationTextBox;
        DialogueManager.Instance.DisplayNextDialogue(DialogueData);
    }
}