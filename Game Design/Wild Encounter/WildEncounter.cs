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
    /// Helper method that determines the type 
    /// of enemy the player will encounter that's in the
    /// <c>BattleCharacterDatas</c> array. If they will not 
    /// encounter any, it returns -1.
    /// </summary>
    /// <returns>Index for the enemy, or -1.</returns>
    private int DetermineEnemyIndex()
    {
        int encounterRate = (int)Random.Range(0f, 100f);
        for (int i = 0; i < EncounterRate.Length; i++)
        {
            if (EncounterRate[i] > encounterRate)
                return i;
        }

        return -1;
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
            enemyEncounter = DetermineEnemyIndex();
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
        if (collider2D.gameObject.CompareTag("Player"))
            inSpawn = true;
    }

    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player"))
            inSpawn = false;
    }
}