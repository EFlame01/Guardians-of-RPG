using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class WildEncounter : MonoBehaviour
{
    public GameObject playerObject;
    public string Environment;
    public int[] EncounterRate;
    public BattleCharacterData BattlePlayerData;
    public BattleCharacterData[] BattleCharacterDatas;

    private bool inSpawn;

    private float time = 0f;
    private bool encounterFound;

    public void Update()
    {
        if (time < 2f)
        {
            time += Time.fixedDeltaTime;
            return;
        }

        time = 0f;
        CheckForEncounter();
    }

    public void CheckForEncounter()
    {
        if (encounterFound)
            return;

        int encounterIndex = EncounterEnemy();
        if (inSpawn && encounterIndex != -1 && GameManager.Instance.PlayerState.Equals(PlayerState.MOVING))
        {
            encounterFound = true;
            BattleCharacterData wildEncounter = BattleCharacterDatas[encounterIndex];
            //DONE: Initiate player and enemy
            BattleInformation.Environment = Environment;
            BattleInformation.BattlePlayerData = BattlePlayerData;
            BattleInformation.BattleEnemiesData[0] = wildEncounter;
            //TODO: check if several enemies have spawned from this wild encounter
            //TODO: check if player has companions who can fight with him
            //TODO: play battle music
            //DONE: start battle
            BattleSimStatus.SceneName = SceneManager.GetActiveScene().name;
            BattleInformation.PlayerPosition = playerObject.transform.position;
            AudioManager.Instance.PlayMusic(Units.Music.WILD_BATTLE_THEME, true);
            SceneLoader.Instance.LoadScene("Battle Scene", TransitionType.WILD_BATTLE);
        }
    }

    public int EncounterEnemy()
    {
        int encounterRate = (int)(UnityEngine.Random.Range(0f, 100f));
        for (int i = 0; i < EncounterRate.Length; i++)
        {
            if (EncounterRate[i] > encounterRate)
                return i;
        }

        return -1;
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player"))
            inSpawn = true;
    }

    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player"))
            inSpawn = false;
    }
}