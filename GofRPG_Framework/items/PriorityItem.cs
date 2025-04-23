using UnityEngine;

///<summary>
/// PriorityItem is a class that extends
/// the Item class. PriorityItems are items
/// that have a probability of making the
/// wielder of the item go first every
/// round.
///</summary>
public class PriorityItem : Item
{
    private int _priorityProb;

    //Constructor
    public PriorityItem(string name, string pluralName, string description, ItemType type, int level, int priorityStage)
    {
        Name = name;
        PluralName = pluralName;
        Description = description;
        Type = type;
        DiscardAfterUse = false;

        _priorityProb = priorityStage switch
        {
            1 => Units.PRIORITY_ITEM_STAGE_1,
            2 => Units.PRIORITY_ITEM_STAGE_2,
            3 => Units.PRIORITY_ITEM_STAGE_3,
            _ => Units.PRIORITY_ITEM_STAGE_3,
        };

    }
    
    ///<summary>
    /// Determines if the <paramref name="character"/> can 
    /// move first. If they can, it will adjust
    /// the <paramref name="character"/>'s turn status to 'MOVE FIRST'.
    ///</summary>
    ///<param name="character"> the character that will be using the item. </param>
    public override void UseItem(Character character)
    {
        int percent = Random.Range(0, 100) + 1;
        if(_priorityProb >= percent)
            character.BattleStatus.SetTurnStatus(TurnStatus.MOVE_FIRST);
        InUse = true;
    }
}