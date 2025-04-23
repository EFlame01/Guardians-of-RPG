
///<summary>
/// KeyItem is a class that extends
/// the Item class. KeyItems have no
/// real benefit apart from being a key
/// item. They are mainly used to progress
/// the plot forward within the game.
///</summary> 
public class KeyItem : Item
{
    //Constructor
    public KeyItem(string name, string pluralName, string description, ItemType type, int level)
    {
        Name = name;
        PluralName = pluralName;
        Description = description;
        Type = type;
        Level = level;
    }

    ///<summary> UseItem let's the game know that the item is now in use. </summary>
    ///<param name="character"> the character that will be using the item. </param>
    public override void UseItem(Character character)
    {
        InUse = true;
    }
}