
///<summary>
/// FoodItem is a class that extends
/// the Item class. A FoodItem essentially
/// heals the character by a set amount
/// of health.
///</summary>
public class FoodItem : Item
{
    private int _healAmount;

    public FoodItem()
    {
    }

    //Constructor

    public FoodItem(string name, string pluralName, string description, ItemType type, int level, int healAmount)
    {
        Name = name;
        PluralName = pluralName;
        Description = description;
        Type = type;
        DiscardAfterUse = true;
        _healAmount = healAmount;
    }

    ///<summary>
    /// Increments the <paramref name="character"/>'s base hp by the _healAmount.
    ///</summary>
    ///<param name="character"> the character that will be using the item. </param>
    public override void UseItem(Character character)
    {
        int trueHealAmount = _healAmount;

        if(_healAmount == -1)
            trueHealAmount = (int)(character.BaseStats.FullHp * 0.1);
        else if(_healAmount == -2)
            trueHealAmount = (int)(character.BaseStats.FullHp * 0.25);
        else if (_healAmount == -3)
            trueHealAmount = (int)(character.BaseStats.FullHp * 0.33);
        else if (_healAmount == -4)
            trueHealAmount = (int)(character.BaseStats.FullHp * 0.41);
        else if (_healAmount == -5)
            trueHealAmount = (int)(character.BaseStats.FullHp * 0.50);
        else if (_healAmount == -6)
            trueHealAmount = (int)(character.BaseStats.FullHp * 0.75);
        else if (_healAmount == -7)
            trueHealAmount = character.BaseStats.FullHp;

        character.BaseStats.SetHp(character.BaseStats.Hp + trueHealAmount);
        InUse = true;
    }
}