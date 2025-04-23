
///<summary>
/// MedicalItem is a class that extends
/// the Item class. MedicalItem works by
/// healing the character's health, and/or
/// curing the character of any status
/// condition.
///</summary>
public class MedicalItem : Item
{
    public int _healAmount {get; private set;}
    public string[] _statusConditions {get; private set;}

    //Constructor
    public MedicalItem(string name, string pluralName, string description, ItemType type, int level, int healAmount, string[] statusConditions)
    {
        Name = name;
        PluralName = pluralName;
        Description = description;
        Type = type;
        DiscardAfterUse = true;
        _healAmount = healAmount;
        _statusConditions = statusConditions;
    }

    ///<summary>
    /// Restores the <paramref name="character"/>'s base HP
    /// either by a fixed amount or by half/all
    /// their HP. If possible, it will also cure the
    /// <paramref name="character"/>'s status conditions if character
    /// has one.
    ///</summary>
    ///<param name="character"> the character that will be using the item. </param>
    public override void UseItem(Character character)
    {
        HealPlayer(character);
        CureStatusCondition(character);
        InUse = true;
    }

    private void HealPlayer(Character character)
    {
        int hp = character.BaseStats.Hp;
        int fullHp = character.BaseStats.FullHp;
        switch(_healAmount)
        {
            case Units.HEAL_1:
                hp += (int)(fullHp * 0.1);
                break;
            case Units.HEAL_2:
                hp += (int)(fullHp * 0.25);
                break;
            case Units.HEAL_3:
                hp += (int)(fullHp * 0.33);
                break;
            case Units.HEAL_4:
                hp += (int)(fullHp * 0.5);
                break;
            case Units.HEAL_5:
                hp += (int)(fullHp * 0.66);
                break;
            case Units.HEAL_6:
                hp += (int)(fullHp * 0.75);
                break;
            case Units.HEAL_7:
                hp = fullHp;
                break;
            default:
                hp += _healAmount;
                break;
        }
        character.BaseStats.SetHp(hp);
    }

    private void CureStatusCondition(Character character)
    {
        if(!CanCureStatusConditions(character))
            return;
        if(_statusConditions[0] == "ALL")
        {
            character.BattleStatus.StatusConditions.Clear();
            return;
        }
        
        for(int i = 0; i < _statusConditions.Length; i++)
        {
            character.BattleStatus.StatusConditions.Remove(_statusConditions[i]);
        }
    }

    public bool CanCureStatusConditions(Character character)
    {
        if(_statusConditions[0] == "ALL")
            return true;
        foreach(string status in _statusConditions)
        {
            if(character.BattleStatus.StatusConditions.ContainsKey(status))
                return true;
        }
        return false;
    }
}