using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;

public class IntroScene : MonoBehaviour
{
    //Serialized variables
    [SerializeField] private DialogueData introDialogueData;
    [SerializeField] private InputActionReference Select;
    [SerializeField] private GameObject[] introUIs;

    //private variables
    public static Story CurrentStory { get; private set; }
    public static int CurrentState { get; private set; }
    public static string SexName { get; private set; }
    public static string ClassName { get; set; }
    public static string ArchetypeName { get; set; }
    public static string PlayerName { get; set; }

    private Player player;
    private const int INTRO_2_UI_INDEX = 0;
    private const int INTRO_3_UI_INDEX = 1;
    private const int INTRO_4_UI_INDEX = 2;
    private const int INTRO_5_UI_INDEX = 3;
    private const int INTRO_6_UI_INDEX = 4;

    public void Start()
    {
        player = Player.Instance();
        DeactivateGameObjects();
        StartDialogue();
    }

    // public virtual void Update()
    // {
    //     if (Select.action.ReadValue<float>() <= 0f)
    //         return;
    //     // if (!_textBoxOpened)
    //     //     return;
    //     if (GameManager.Instance.EnableNarrationInputs)
    //         OnNextButtonPressed();
    // }

    public void OnNextButtonPressed()
    {
        if (CurrentStory == null)
            return;

        CurrentState = (int)CurrentStory.variablesState["currentState"];

        if (DialogueManager.Instance.DialogueEnded)
        {
            SetUpPlayerInformation();
            StartGame();
        }

        switch (CurrentState)
        {
            case 1:
                DeactivateGameObjects();
                break;
            case 2:
                SetIntroUI(INTRO_2_UI_INDEX);
                break;
            case 3:
                if (SexName != null && SexName.Length > 0)
                    SetIntroUI(INTRO_3_UI_INDEX);
                else
                    SetIntroUI(INTRO_2_UI_INDEX);
                break;
            case 4:
                if (ClassName != null && ClassName.Length > 0)
                    SetIntroUI(INTRO_4_UI_INDEX);
                else
                    SetIntroUI(INTRO_3_UI_INDEX);
                break;
            case 5:
                if (ArchetypeName != null && ArchetypeName.Length > 0)
                    SetIntroUI(INTRO_5_UI_INDEX);
                else
                    SetIntroUI(INTRO_4_UI_INDEX);
                break;
            case 6:
                if (PlayerName != null && PlayerName.Length > 0)
                    SetIntroUI(INTRO_6_UI_INDEX);
                else
                    SetIntroUI(INTRO_5_UI_INDEX);
                break;
            default:
                SetUpPlayerInformation();
                StartGame();
                break;
        }
    }

    public void OnBackButtonPressed()
    {
        if (CurrentStory == null)
            return;

        CurrentStory.variablesState["stateStatus"] = "back";

        switch (CurrentState)
        {
            case 0:
                SetCurrentState(0);
                RemovePlayerInformation();
                SendToStartScene();
                break;
            case 1:
                SetCurrentState(0);
                Start();
                break;
            case 2:
                SexName = "";
                SetCurrentState(0);
                player.SetSex(null);
                StartDialogue();
                DeactivateGameObjects();
                break;
            case 3:
                ClassName = "";
                SetCurrentState(2);
                StartDialogue();
                SetIntroUI(INTRO_2_UI_INDEX);
                break;
            case 4:
                ArchetypeName = "";
                SetCurrentState(3);
                player.SetArchetype(null);  //TODO: does not actually null archetype...
                StartDialogue();
                SetIntroUI(INTRO_3_UI_INDEX);
                break;
            case 5:
                name = "";
                SetCurrentState(4);
                player.SetName(null);       //TODO: does not actually null name...
                StartDialogue();
                SetIntroUI(INTRO_4_UI_INDEX);
                break;
            case 6:
                SetCurrentState(5);
                SetIntroUI(INTRO_5_UI_INDEX);
                break;
        }
    }

    public void OnSexSelected(string sex)
    {
        SexName = sex;
        CurrentStory.variablesState["stateStatus"] = "next";
    }

    private void SetIntroUI(int uiIndex)
    {
        CurrentStory.variablesState["stateStatus"] = "";
        DeactivateGameObjects(uiIndex);
        ActivateGameObject(uiIndex);
    }

    private void DeactivateGameObjects()
    {
        foreach (GameObject go in introUIs)
            go.SetActive(false);
    }

    private void DeactivateGameObjects(int uiIndex)
    {
        for (int i = 0; i < introUIs.Length; i++)
        {
            if (i != uiIndex)
                introUIs[i].SetActive(false);
        }
    }

    private void ActivateGameObject(int index)
    {
        introUIs[index].SetActive(true);
    }

    private void SetCurrentState(int state)
    {
        CurrentState = state;
        CurrentStory.variablesState["currentState"] = state;
    }

    private void StartDialogue()
    {
        DialogueManager.Instance.DisplayNextDialogue(introDialogueData);
        CurrentStory = DialogueManager.Instance.CurrentStory;
    }

    private void SetUpPlayerInformation()
    {
        Ability ability = AbilityMaker.Instance.GetAbilityBasedOnName("Empyrean Binary");
        Move[] moves = MoveMaker.Instance.GetLevelUpMoves(player.Level, ArchetypeName, ClassName);

        player.SetName(PlayerName);
        player.SetSex(SexName);
        player.SetArchetype(ArchetypeName);
        player.SetAbility(ability);
        player.SetBaseStats(
            player.Archetype.BaseStats.FullHp,
            player.Archetype.BaseStats.Atk,
            player.Archetype.BaseStats.Def,
            player.Archetype.BaseStats.Eva,
            player.Archetype.BaseStats.Hp,
            player.Archetype.BaseStats.Spd,
            Units.BASE_ELX,
            Units.BASE_ACC,
            Units.BASE_CRT
        );
        foreach (Move move in moves)
            player.MoveManager.AddMove(move);

        player.StoryFlagManager.AddAllStoryFlags();

        TimeTracker.Instance().SetTotalSavedPlayTime(0);
        TimeTracker.Instance().StartTime();
    }

    private void RemovePlayerInformation()
    {
        player.NullPlayer();
        player = Player.Instance();
    }

    private void SendToStartScene()
    {
        SceneLoader.Instance.LoadScene("Start Scene", TransitionType.FADE_TO_BLACK);
    }

    private void StartGame()
    {
        ChapterScene.PartName = "Part 1: Grace Land";
        ChapterScene.ChapterName = "Chapter 1 - " + PlayerName + " and the Bear";
        ChapterScene.SceneName = "Forest 1";
        SceneLoader.Instance.LoadScene("Chapter Scene", TransitionType.FADE_TO_BLACK);
    }
}