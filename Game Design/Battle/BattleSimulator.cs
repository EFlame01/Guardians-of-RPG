using System;
using UnityEngine;

/// <summary>
/// BattleSimulator is a class that controls the 
/// <c>BattleStateMachine</c> and all the other 
/// <c>Characters</c> in the battle.
/// </summary>
public class BattleSimulator : MonoBehaviour
{
    //BattleStateMachine
    public BattleStateMachine BattleStateMachine;

    //serialized variables
    [SerializeField] private EnvironmentDetail[] EnvironmentDetails;
    [SerializeField] private Camera Camera;
    [SerializeField] private TextBox NarrationTextBox;
    [SerializeField] private DialogueData DialogueData;
    [SerializeField] private BattleActionEffect BattleActionEffect;

    //BattleCharacters to initialize
    [SerializeField] private BattleCharacter BattlePlayer;
    [SerializeField] private BattleCharacter[] BattleAllies = new BattleCharacter[2];
    [SerializeField] private BattleCharacter[] BattleEnemies = new BattleCharacter[3];

    //UI for battle options
    [SerializeField] private GameObject BattleOptions;
    [SerializeField] private GameObject BattleTimer;

    //UI for status conditions
    [SerializeField] private GameObject BurnSymbol;
    [SerializeField] private GameObject FrozenSymbol;
    [SerializeField] private GameObject PetrifiedSymbol;
    [SerializeField] private GameObject PoisonSymbol;
    [SerializeField] private GameObject SleepSymbol;
    [SerializeField] private GameObject StunSymbol;

    //ALL BATTLE STATES
    private BattleState InitializeState;
    private BattleState OptionState;
    private BattleState CharacterActionState;
    private BattleState ActionEffectState;
    private BattleState KnockoutState;
    private BattleState AfterRoundState;
    private BattleState BattleOverState;

    private void Awake()
    {
        BattleStateMachine = new BattleStateMachine();
        InitializeState = new InitializeState(BattlePlayer, BattleAllies, BattleEnemies, EnvironmentDetails, Camera, DialogueData, NarrationTextBox);
        OptionState = new OptionState(BattleTimer, BattleOptions);
        CharacterActionState = new CharacterActionState(DialogueData, NarrationTextBox);
        ActionEffectState = new ActionEffectState(BattlePlayer, BattleAllies, BattleEnemies, Camera, DialogueData, NarrationTextBox, BattleActionEffect);
        KnockoutState = new KnockoutState(DialogueData, NarrationTextBox);
        AfterRoundState = new AfterRoundState();
        BattleOverState = new BattleOverState((PlayerHUD)BattlePlayer.CharacterHUD, DialogueData, NarrationTextBox);
    }

    // Start is called before the first frame update
    private void Start()
    {
        try
        {
            DayNightCycle.Instance.StopTimer();
        }
        catch (Exception e)
        {
            Debug.LogWarning("WARNING: " + e.ToString());
        }
        BattleStateMachine.StartState(InitializeState);
        BattleSimStatus.AssignStatusGameObjects(BurnSymbol, FrozenSymbol, PetrifiedSymbol, PoisonSymbol, SleepSymbol, StunSymbol);
    }

    // Update is called once per frame
    private void Update()
    {
        BattleStateMachine.CurrentState.Update();
        CheckState(BattleStateMachine.CurrentState.NextState);
    }

    private void CheckState(string nextState)
    {
        if (nextState == null)
            return;

        switch (nextState)
        {
            case "OPTION STATE":
                UpdateElixirPool();
                BattleStateMachine.ChangeState(OptionState);
                break;
            case "CHARACTER ACTION STATE":
                BattleStateMachine.ChangeState(CharacterActionState);
                break;
            case "ACTION EFFECT STATE":
                BattleStateMachine.ChangeState(ActionEffectState);
                break;
            case "ACTION EFFECT 2 STATE":
                break;
            case "KNOCK OUT STATE":
                BattleStateMachine.ChangeState(KnockoutState);
                break;
            case "AFTER ROUND STATE":
                BattleStateMachine.ChangeState(AfterRoundState);
                break;
            case "BATTLE OVER STATE":
                // BattleOverState = new BattleOverState((PlayerHUD)BattlePlayer.CharacterHUD, DialogueData, NarrationTextBox);
                BattleStateMachine.ChangeState(BattleOverState);
                break;
            case "END BATTLE":
                EndBattle();
                break;
            default:
                Debug.Log(nextState);
                break;
        }
    }

    private void EndBattle()
    {
        bool didPlayerWin = BattleSimStatus.DidPlayerWin;
        bool gameOverScreen = BattleSimStatus.GameOverScreenIfLost;

        //Reset Elx and Stats
        Player.Instance().BaseStats.ResetStats();

        //Reset Health if HP = 0
        if (Player.Instance().BaseStats.Hp == 0)
            Player.Instance().BaseStats.ResetHealth();

        TextBoxBattle.KeepTextBoxOpened = false;
        TextBoxBattle.EndNarrationNow = true;

        //Take player back to original scene
        string sceneName = BattleSimStatus.SceneName;
        PlayerSpawn.PlayerPosition = BattleInformation.PlayerPosition;
        BattleSimStatus.ClearBattleSimStatus();
        BattleStateMachine.EndStateMachine();
        BattleInformation.ResetBattleInformation();
        try
        {
            DayNightCycle.Instance.ResumeTimer();
        }
        catch (Exception e)
        {
            Debug.LogWarning("WARNING: " + e.ToString());
        }
        //TODO: create method in sceneloader to fade music out
        AudioManager.Instance.StopCurrentMusic(false);
        if (!didPlayerWin && gameOverScreen)
            SceneLoader.Instance.LoadScene("Game Over Scene", TransitionType.FADE_TO_BLACK);
        else
            SceneLoader.Instance.LoadScene(sceneName, TransitionType.FADE_TO_BLACK);
    }

    private void UpdateElixirPool()
    {
        //BattlePlayer
        BattlePlayer.Character.BaseStats.RegenElx(BattlePlayer.Character.GetAverageElixirCost());
        BattlePlayer.UpdateHUD();

        //BattleAllies
        foreach (BattleCharacter battleAlly in BattleAllies)
        {
            if (battleAlly.Character != null && battleAlly.Character.BaseStats.Hp > 0)
            {
                battleAlly.Character.BaseStats.RegenElx(battleAlly.Character.GetAverageElixirCost());
                battleAlly.UpdateHUD();
            }
        }

        //BattleEnemies
        foreach (BattleCharacter battleEnemy in BattleEnemies)
        {
            if (battleEnemy.Character != null && battleEnemy.Character.BaseStats.Hp > 0)
            {
                battleEnemy.Character.BaseStats.RegenElx(battleEnemy.Character.GetAverageElixirCost());
                battleEnemy.UpdateHUD();
            }
        }
    }
}