
using UnityEngine;

/// <summary>
/// CharacterMaker is a class that parses through
/// the data to create <c>Character</c> objects for
/// </summary>
public class CharacterMaker : Singleton<CharacterMaker>
{
    private const int CHARACTER_INDEX = 2;
    private const int CHARACTER_DATA_ROW_LENGTH = 13;
    public Character UNKNOWN_CHARACTER = new Character("unkonwn_character", "Unknown NPC", "NONE", 1, 500, "NONE", "MALEFE", new Move[4], new int[5] { 5, 5, 5, 5, 5 }, null, null);


    public Character GetCharacterBasedOnName(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            Debug.LogWarning($"WARNING: The character name, [ {name} ], was null or empty. Could not retrieve character data.");
            return null;
        }

        Character character;
        string characterDataRow = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[CHARACTER_INDEX], name);
        int characterDataLength = characterDataRow.Split(',').Length;
        if (string.IsNullOrEmpty(characterDataRow) || characterDataLength != CHARACTER_DATA_ROW_LENGTH)
        {
            Debug.LogWarning($"WARNING: An error occured when trying to retrieve {name}, so the data returned a generic NPC. Please report bug if found.\n- {characterDataRow}\n- length: {characterDataLength}");
            character = UNKNOWN_CHARACTER;
        }
        else
        {
            string[] characterAttributes = characterDataRow.Split(',');

            character = new Character
            (
                characterAttributes[0],//id
                characterAttributes[1],//name
                characterAttributes[3],//type
                int.TryParse(characterAttributes[2], out int level) ? level : 1,//level
                int.TryParse(characterAttributes[4], out int bits) ? bits : 0,//gold (bits)
                characterAttributes[5],//archetype
                characterAttributes[6],//sex
                new Move[]
                        {
                            MoveMaker.Instance.GetMoveBasedOnName(characterAttributes[7]),
                            MoveMaker.Instance.GetMoveBasedOnName(characterAttributes[8]),
                            MoveMaker.Instance.GetMoveBasedOnName(characterAttributes[9]),
                            MoveMaker.Instance.GetMoveBasedOnName(characterAttributes[10])
                        },//moves (7,8,9,10)
                new int[5] { 5, 5, 5, 5, 5 },//stats 
                AbilityMaker.Instance.GetAbilityBasedOnName(characterAttributes[11]),//ability
                ItemMaker.Instance.GetItemBasedOnName(characterAttributes[12])//item
            );
        }

        //Update stats for character
        for (int i = 2; i <= character.Level; i++)
        {
            character.BaseStats.LevelUpStats(character.Archetype.ChooseStatBoostRandomly());
        }

        return character;
    }
}