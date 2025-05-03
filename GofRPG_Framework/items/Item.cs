using UnityEngine;

///<summary>
/// Item is an abstract class that is
/// created so characters can use an
/// item and it's effects.
///</summary>
public class Item
{
    public string Name {get; protected set;}
    public string PluralName {get; protected set;}
    public string Description {get; protected set;}
    public ItemType Type {get; protected set;}
    public int Level {get; protected set;}
    public bool DiscardAfterUse {get; protected set;}
    public bool InUse {get; protected set;}
    public int Price {get; protected set;}

    ///<summary> 
    /// The <paramref name="character"/> uses the item.
    /// The item's effect will vary depending on the type of item.
    ///</summary>
    ///<param name="character"> the character using the item. </param>
    public virtual void UseItem(Character character)
    {
        //nothing.
    }
}