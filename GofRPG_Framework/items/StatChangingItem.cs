using UnityEngine;

///<summary>
/// StatChangingItem is a class that extends the 
/// Item class. StatChangingItems change the stats
/// of the user by a FIXED static amount once the 
/// user equips the item.
///</summary>
public class StatChangingItem : Item
{
    public string[] _stats {get; private set;}
    public int[] _values {get; private set;}

    //Constructor
    public StatChangingItem(string name, string pluralName, string description, ItemType type, int level, string[] stats, int[] values)
    {
        Name = name;
        PluralName = pluralName;
        Description = description;
        Type = type;
        DiscardAfterUse = true;
        _stats = stats;
        _values = values;
    }

    /// <summary>
    /// Updates the stats of the <paramref name="character"/>
    /// </summary>
    /// <param name="character"></param>
    public override void UseItem(Character character)
    {
        for(int i = 0; i < _stats.Length; i++)
        {
            switch(_stats[i])
            {
                case "ATK":
                    character.BaseStats.SetAtk(character.BaseStats.Atk + _values[i]);
                    break;
                case "Def":
                    character.BaseStats.SetDef(character.BaseStats.Def + _values[i]);
                    break;
                case "EVA":
                    character.BaseStats.SetEva(character.BaseStats.Eva + _values[i]);
                    break;
                case "HP":
                    character.BaseStats.SetHp(character.BaseStats.Hp + _values[i]);
                    character.BaseStats.SetFullHp(character.BaseStats.FullHp + _values[i]);
                    break;
                case "SPD":
                    character.BaseStats.SetSpd(character.BaseStats.Spd + _values[i]);
                    break;
                default:
                    break;
            }
        }
    }
}