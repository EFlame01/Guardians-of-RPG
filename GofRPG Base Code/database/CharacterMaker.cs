using System;
using UnityEngine;

/// <summary>
/// CharacterMaker is a class that parses through
/// the data to create <c>Character</c> objects for
/// </summary>
public class CharacterMaker : Singleton<CharacterMaker>
{
    private readonly string _characterDatabasePath = "/database/characters.csv";

    public Character GetCharacterBasedOnName(string name)
    {
        if(name == null) 
            return null;

        Character character = null;
        string[] characterAttributes;
        DataEncoder.Instance.DecodeFile(_characterDatabasePath);
        characterAttributes = DataEncoder.Instance.GetRowOfData(name).Split(',');
        DataEncoder.ClearData();

        Move[] moveArray = 
        {
            MoveMaker.Instance.GetMoveBasedOnName(characterAttributes[7]),
            MoveMaker.Instance.GetMoveBasedOnName(characterAttributes[8]),
            MoveMaker.Instance.GetMoveBasedOnName(characterAttributes[9]),
            MoveMaker.Instance.GetMoveBasedOnName(characterAttributes[10])
        };
        int[] statArray = 
        {
            5, 
            5, 
            5,
            5,
            5,
        };
        character = new Character
        (
            characterAttributes[0],//id
            characterAttributes[1],//name
            characterAttributes[3],//type
            Int32.Parse(characterAttributes[2]),//level
            Int32.Parse(characterAttributes[4]),//gold (bits)
            characterAttributes[5],//archetype
            characterAttributes[6],//sex
            moveArray,//moves (7,8,9,10)
            statArray,//stats 
            AbilityMaker.Instance.GetAbilityBasedOnName(characterAttributes[11]),//ability
            ItemMaker.Instance.GetItemBasedOnName(characterAttributes[12])//item
        );

        //Update stats for character
        for(int i = 2; i <= character.Level; i++)
        {
            character.BaseStats.LevelUpStats(character.Archetype.ChooseStatBoostRandomly());
        }

        //TODO: Test. Delete Later
        // character.BattleStatus.StatusConditions["BURN"] = new Burn();
        // character.BattleStatus.StatusConditions["POISON"] = new Poison(1);

        return character;
    }
}