using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// FightState is a class that extends from the 
/// <c>CutSceneState</c> class. FightState
/// initializes everything needed to start a 
/// battle, ends the cut scene, and begins
/// the battle simulation.
/// </summary>
public class FightState : CutSceneState
{
    [Header("Battle System Data")]
    public string Environment;
    public BattleCharacterData BattlePlayerData;
    public BattleCharacterData[] BattleAlliesData;
    public BattleCharacterData[] BattleEnemiesData;

    [Header("Transition")]
    public TransitionType TransitionType;

    [Header("Story Flags")]
    public string[] StoryFlagsIfWon;

    [Header("Music")]
    public string TrackName;

    [Header("Lose Battle Information")]
    public bool GameOverScreen;
    public string SceneName;
    public string LoseMessage;
    public Vector3 Position = new Vector3(0, 0, 0);

    public override void Enter()
    {
        base.Enter();
        SetStoryFlagsInCutScene();
        SetUpBattleMusic();
        SetUpForBattle();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    /// <summary>
    /// Sets up battle music
    /// </summary>
    private void SetUpBattleMusic()
    {
        AudioManager.Instance.PlayMusic(TrackName, true);
    }

    /// <summary>
    /// Sets up everything needed for battle:
    /// <list> - Battle Environment </list>
    /// <list> - Player Data </list>
    /// <list> - Enemy Data </list>
    /// <list> - Ally Data </list>
    /// <list> - Story Flags </list>
    /// </summary>
    private void SetUpForBattle()
    {
        BattleInformation.Environment = Environment;
        BattleInformation.BattlePlayerData = BattlePlayerData;
        BattleInformation.PlayerPosition = transform.position;
        BattleInformation.StoryFlagsIfWon = StoryFlagsIfWon;

        BattleSimStatus.GameOverScreenIfLost = GameOverScreen;

        GameOverScene.Instance.SetScene(LoseMessage, SceneName, Position);

        for (int i = 0; i < BattleAlliesData.Length; i++)
        {
            if (BattleAlliesData[i] != null)
                BattleInformation.BattleAlliesData[i] = BattleAlliesData[i];
        }

        for (int i = 0; i < BattleEnemiesData.Length; i++)
        {
            if (BattleEnemiesData[i] != null)
                BattleInformation.BattleEnemiesData[i] = BattleEnemiesData[i];
        }

        BattleSimStatus.CanRun = false;
        BattleSimStatus.SceneName = SceneManager.GetActiveScene().name;
        SceneLoader.Instance.LoadScene("Battle Scene", TransitionType);
    }
}