
/// <summary>
/// CharacterMaker is a class that parses through
/// the data to create <c>Character</c> objects for
/// </summary>
public class CharacterMaker : Singleton<CharacterMaker>
{
    private const int CHARACTER_INDEX = 2;

    public Character GetCharacterBasedOnName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        Character character;
        string[] characterAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[CHARACTER_INDEX], name).Split(',');
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
            int.Parse(characterAttributes[2]),//level
            int.Parse(characterAttributes[4]),//gold (bits)
            characterAttributes[5],//archetype
            characterAttributes[6],//sex
            moveArray,//moves (7,8,9,10)
            statArray,//stats 
            AbilityMaker.Instance.GetAbilityBasedOnName(characterAttributes[11]),//ability
            ItemMaker.Instance.GetItemBasedOnName(characterAttributes[12])//item
        );

        //Update stats for character
        for (int i = 2; i <= character.Level; i++)
        {
            character.BaseStats.LevelUpStats(character.Archetype.ChooseStatBoostRandomly());
        }

        return character;
    }
}