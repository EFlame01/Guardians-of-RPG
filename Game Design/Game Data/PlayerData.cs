using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// PlayerData is a class that holds
/// the meta data for the <c>Player</c>
/// that needs to  be saved and loaded
/// back into the player/game.
/// </summary>
[System.Serializable]
public class PlayerData
{
    //DEFAULT CHARACTER INFORMATION
    public  string name;
    public  int level;
    public  int gold;
    public  string archetypeName;
    public  string sex;

    public  string[] battleMoves = new string[4];
    
    public  int fullHP;
    public  int atk;
    public  int def;
    public  int eva;
    public  int hp;
    public  int spd;
    public  double acc;
    public  double crt;

    public  string abilityName;
    
    public  string equippedItemName;

    //PLAYER INFORMATION
    public  int currentXP;
    public  int limitXP;

    public  string[] movesLearned;
    public  string[] abilityNames;
    // public  string[,] inventory;

    public string sceneName;
    public string mapLocationName;
    public Vector3 locationPosition;

    //Constructor
    public PlayerData()
    {
        Player p = Player.Instance();

        name = p.Name;
        level = p.Level;
        gold = p.Bits;
        archetypeName = p.Archetype.ArchetypeName;
        sex = p.Sex;
        
        for(int i = 0; i < battleMoves.Length; i++)
            battleMoves[i] = p.BattleMoves[i] ? .Name;
        
        fullHP = p.BaseStats.FullHp;
        atk = p.BaseStats.Atk;
        def = p.BaseStats.Def;
        eva = p.BaseStats.Eva;
        hp = p.BaseStats.Hp;
        spd = p.BaseStats.Spd;
        acc = p.BaseStats.Acc;
        crt = p.BaseStats.Crt;

        abilityName = p.Ability ? .Name;
        equippedItemName = p.Item ? .Name;

        currentXP = p.CurrXP;
        limitXP = p.LimXP;

        movesLearned = MoveManager.MoveDictionary.Keys.ToArray();
        abilityNames = AbilityManager.AbilityDictionary.Keys.ToArray();

        sceneName = SceneManager.GetActiveScene().name;
        mapLocationName = MapLocation.GetCurrentMapLocation();
        locationPosition = PlayerSpawn.PlayerPosition;
    }

    /// <summary>
    /// Loads the Serialized PlayerData back
    /// into the player and game.
    /// </summary>
    public void LoadPlayerDataIntoGame()
    {
        Player p = Player.Instance();

        p.SetName(name);
        p.SetLevel(level);
        p.SetBits(gold);
        p.SetArchetype(archetypeName);
        p.SetSex(sex);
        p.SetBattleMoves(battleMoves);
        p.MoveManager.AddToMovesLearned(movesLearned);
        p.SetBaseStats(fullHP, atk, def, eva, hp, spd, acc, crt);
        p.SetAbility(AbilityMaker.Instance.GetAbilityBasedOnName(abilityName));
        p.SetItem(ItemMaker.Instance.GetItemBasedOnName(equippedItemName));
        p.SetCurrentXP(currentXP);
        p.SetLimitXP(limitXP);
        p.AbilityManager.AddAbilitiesToList(abilityNames);
        PlayerSpawn.PlayerPosition = locationPosition;

        MapLocation.SetCurrentMapLocation(mapLocationName);
    }
}