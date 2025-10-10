using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// WildEncounter is a class that creates 
/// and calculates the logic for wild encounters
/// in the game. This includes the level of 
/// wild encounters, as well as the level of your
/// companions.
/// </summary>
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
        //Check for encounter every 2 seconds
        if (time < 2f)
            time += Time.deltaTime;
        else
        {
            time = 0f;
            CheckForEncounter();
        }
    }

    /// <summary>
    /// Checks if the player has ran into an encounter.
    /// if they have, it will calculate how many, and 
    /// if the player will be able to fight with any 
    /// companions.
    /// </summary>
    public void CheckForEncounter()
    {
        //if we have already found an encounter
        if (encounterFound)
            return;

        //check if player is in spawn and is moving before
        if (!inSpawn || !GameManager.Instance.PlayerState.Equals(PlayerState.MOVING))
            return;

        //find enemies for all three times
        if (DetermineEnemySpawn() == 0)
            return;
        else
            encounterFound = true;

        //find companions for player to fight with
        DetermineCompanionSpawn();

        //configure the rest of the battle simulator
        ConfigureBattleInformation();

        //enter battle scene
        SceneLoader.Instance.LoadScene("Battle Scene", TransitionType.WILD_BATTLE);
    }

    /// <summary>
    /// Determines how many emeies will spawn from the 
    /// wild encounter and adds that information to the
    /// <c>BattleInformation</c> class.
    /// </summary>
    /// <returns>The number of enemies that will spawn.</returns>
    private int DetermineEnemySpawn()
    {
        int enemyEncounter;
        int enemyIndex = 0;
        for (int i = 0; i < 3; i++)
        {
            enemyEncounter = DetermineEnemyIndex(i);
            if (enemyEncounter == -1)
                continue;
            else
            {
                BattleCharacterData wildEncounter = BattleCharacterDatas[enemyEncounter];
                BattleInformation.BattleEnemiesData[enemyIndex++] = wildEncounter;
            }
        }

        return enemyIndex;
    }

    /// <summary>
    /// Helper method that determines the type 
    /// of enemy the player will encounter that's in the
    /// <c>BattleCharacterDatas</c> array. If they will not 
    /// encounter any, it returns -1.
    /// </summary>
    /// <returns>Index for the enemy, or -1.</returns>
    private int DetermineEnemyIndex()
    {
        int encounterRate = Random.Range(0, 100) + 1;
        return DetermineEnemyIndexHelper(encounterRate);
    }

    /// <summary>
    /// This overloaded method changes the odds in
    /// which more enemies spawn.
    /// </summary>
    /// <param name="indexMultiplyer">The index in the forloop for enemies to spawn.</param>
    /// <returns></returns>
    private int DetermineEnemyIndex(double indexMultiplyer)
    {
        if (indexMultiplyer == 0)
            indexMultiplyer = Units.STAGE_0;
        else
            indexMultiplyer *= Units.STAGE_POS_1;

        int encounterRate = (int)(Random.Range(0f, 100f) * indexMultiplyer);
        return DetermineEnemyIndexHelper(encounterRate);
    }

    private int DetermineEnemyIndexHelper(int encounterRate)
    {
        List<int> indexList = new List<int>();

        for (int i = 0; i < EncounterRate.Length; i++)
        {
            if (EncounterRate[i] > encounterRate)
                indexList.Add(i);
        }

        if (indexList.Count == 0)
            return -1;

        return indexList[Random.Range(0, indexList.Count)];
    }

    /// <summary>
    /// Determines how many allies will assist 
    /// the playerand adds that information to the
    /// <c>BattleInformation</c> class.
    /// </summary>
    private void DetermineCompanionSpawn()
    {
        BattleInformation.BattlePlayerData = BattlePlayerData;
        BattleCharacterData[] companionData = PlayerCompanions.Instance.CompanionData;
        int companionIndex = 0;
        for (int i = 0; i < companionData.Length; i++)
        {
            if (companionData[i] != null)
                BattleInformation.BattleAlliesData[companionIndex++] = companionData[i];
        }
    }

    /// <summary>
    /// Configures everything else needed to 
    /// begin the <c>BattleSimulator</c>
    /// </summary>
    private void ConfigureBattleInformation()
    {
        BattleInformation.Environment = Environment;
        BattleSimStatus.SceneName = SceneManager.GetActiveScene().name;
        BattleInformation.PlayerPosition = playerObject.transform.position;
        AudioManager.Instance.PlayMusic(Units.Music.WILD_BATTLE_THEME, true);
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Grass"))
            inSpawn = true;
    }

    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Grass"))
            inSpawn = false;
    }
}