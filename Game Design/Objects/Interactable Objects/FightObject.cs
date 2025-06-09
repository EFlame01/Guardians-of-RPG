using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// FightObject is a class that extends the 
/// <c>InteractableObject</c> class. FightObject
/// Will either have the ability to detect the <c>Player</c>
/// walking along it's field of vision, or once
/// the player interacts with it, it will begin
/// to initiate a battle with the player
/// </summary>
public class FightObject : NPCObject
{
    [Header("FightObject Properties")]
    [SerializeField] public string Environment;
    [SerializeField] public BattleCharacterData BattlePlayerData;
    [SerializeField] public BattleCharacterData[] BattleAlliesData;
    [SerializeField] public BattleCharacterData[] BattleEnemiesData;
    [Header("Transition")]
    [SerializeField] public TransitionType TransitionType;
    [Header("Story Flags")]
    [SerializeField] public bool startedeBattle;
    [SerializeField] public string[] StoryFlagsIfWon;
    [Header("Music")]
    [SerializeField] public string TrackName;

    public override void InteractWithObject()
    {
        if (CanInteract)
            StartCoroutine(ConfrontPlayer());
    }

    public IEnumerator ConfrontPlayer()
    {
        TurnToPlayer();
        StartDialogue();
        while(!DialogueManager.Instance.DialogueEnded)
            yield return null;
        StartFight();
    }

    public void StartFight()
    {
        SetUpBattleMusic();
        SetUpForBattle();
    }

    private void SetUpBattleMusic()
    {
        AudioManager.Instance.PlayMusic(TrackName, true);
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
