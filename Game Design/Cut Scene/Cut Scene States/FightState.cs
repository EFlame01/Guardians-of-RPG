using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] public string Environment;
    [SerializeField] public BattleCharacterData BattlePlayerData;
    [SerializeField] public BattleCharacterData[] BattleAlliesData;
    [SerializeField] public BattleCharacterData[] BattleEnemiesData;
    [SerializeField] public TransitionType TransitionType;
    [SerializeField] public string[] StoryFlagsIfWon;

    public override void Enter()
    {
        base.Enter();
        SetStoryFlagsInCutScene();
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

    private void SetUpForBattle()
    {
        BattleInformation.Environment = Environment;
        BattleInformation.BattlePlayerData = BattlePlayerData;
        BattleInformation.PlayerPosition = transform.position;
        BattleInformation.StoryFlagsIfWon = StoryFlagsIfWon;
        
        Debug.Log(PlayerSpawn.PlayerPosition);

        for(int i = 0; i < BattleAlliesData.Length; i++)
        {
            if(BattleAlliesData[i] != null)
                BattleInformation.BattleAlliesData[i] = BattleAlliesData[i];
        }

        for(int i = 0; i < BattleEnemiesData.Length; i++)
        {
            if(BattleEnemiesData[i] != null)
                BattleInformation.BattleEnemiesData[i] = BattleEnemiesData[i];
        }

        BattleSimStatus.SceneName = SceneManager.GetActiveScene().name;
        SceneLoader.Instance.LoadScene("Battle Scene", TransitionType);
    }
}