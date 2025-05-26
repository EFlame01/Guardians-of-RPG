using UnityEngine;

///<summary>
/// Character is a class that
/// holds all the character's basic information
/// needed for the GofRPG_API.
///</summary>
public class Character
{
    public string Id {get; protected set;}
    public string Type {get; protected set;}
    public string Name {get; protected set;}
    public int Level {get; protected set;}
    public int Bits {get; protected set;}
    public Archetype Archetype {get; protected set;}
    public string Sex {get; protected set;}
    public Move[] BattleMoves {get; protected set;}
    public BaseStats BaseStats {get; protected set;}
    public BattleStatus BattleStatus {get; protected set;}
    public Ability Ability {get; protected set;}
    public Item Item {get; set;}

    //Constructor
    public Character(string id, string name, string type, int level, int bits, string archetype, string sex, Move[] battleMoves, int[] stats, Ability ability, Item item)
    {
        Id = id; 
        Name = name;
        Type = type;
        Level = level;
        Bits = bits;
        Archetype = Archetype.GetArchetype(archetype);
        Sex = sex;
        //TODO: generate move from list of moves based off name
        BattleMoves = battleMoves;
        BaseStats = new BaseStats(stats[0], stats[1], stats[2], stats[3], stats[4]);
        BattleStatus = new BattleStatus();
        Ability = ability;
        Item = item;
    }

    //Empty Constructor
    public Character()
    {

    }
    
    //Getters and Setters
    public void SetName(string name)
    {
        Name = name ?? Name;
    }
    public void SetLevel(int level)
    {
        Level = Mathf.Clamp(level, Level, 100);
    }
    public void SetBits(int bits)
    {
        Bits = bits < 0? 0 : bits;
    }
    public void SetArchetype(string archetype)
    {
        Archetype = archetype == null ? Archetype : Archetype.GetArchetype(archetype);
    }
    public void SetSex(string sex)
    {
        Sex = sex ?? "MALEFE";
    }
    public void SetBattleMoves(string[] moves)
    {
        for(int i = 0; i < 4; i++)
            BattleMoves[i] = moves[i] != null ? MoveMaker.Instance.GetMoveBasedOnName(moves[i]) : null;
    }
    public void SetBaseStats(int fullHp, int atk, int def, int eva, int hp, int spd, double acc, double crt)
    {
        BaseStats = new BaseStats(atk, def, eva, hp, spd);
        BaseStats.SetFullHp(fullHp);
        BaseStats.SetAcc(acc);
        BaseStats.SetCrt(crt);
    }
    public void SetAbility(Ability ability)
    {
        Ability ??= ability;
    }
    public void SetItem(Item item)
    {
        Item ??= item;
    }
}