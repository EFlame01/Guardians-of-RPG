using System;

/// <summary>
/// ItemMaker is a class that parses through
/// the data to create <c>Item</c> objects for
/// the <c>Character</c> class.
/// </summary>
public class ItemMaker : Singleton<ItemMaker>
{
    private readonly string _itemDatabasePath = "/database/items.csv";

    /// <summary>
    /// Gets and returns the <c>Item</c> object based on the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the object</param>
    /// <returns>the <c>Item</c> objects or <c>null</c> if the item could not be found.</returns>
   public Item GetItemBasedOnName(string name)
    {
        if(name == null) 
            return null;

        Item item = null;
        string[] itemAttributes;

        DataEncoder.Instance.DecodeFile(_itemDatabasePath);
        itemAttributes = DataEncoder.Instance.GetRowOfData(name).Split(',');
        DataEncoder.ClearData();

        switch(itemAttributes[3])
        {
            case "FOOD":
                item = new FoodItem
                (
                    itemAttributes[0],
                    itemAttributes[1],
                    itemAttributes[2].Replace('~', ','),
                    ItemType.FOOD,
                    int.Parse(itemAttributes[4]),
                    int.Parse(itemAttributes[5])
                );
                break;
            case "HEALING":
                item = new HealingItem
                (
                    itemAttributes[0],
                    itemAttributes[1],
                    itemAttributes[2].Replace('~', ','),
                    ItemType.HEALING,
                    int.Parse(itemAttributes[4]),
                    int.Parse(itemAttributes[6])
                );
                break;
            case "KEY":
                item = new KeyItem
                (
                    itemAttributes[0],
                    itemAttributes[1],
                    itemAttributes[2].Replace('~', ','),
                    ItemType.HEALING,
                    int.Parse(itemAttributes[4])
                );
                break;
            case "MEDICAL":
                item = new MedicalItem
                (
                    itemAttributes[0],
                    itemAttributes[1],
                    itemAttributes[2].Replace('~', ','),
                    ItemType.MEDICAL,
                    int.Parse(itemAttributes[4]),
                    int.Parse(itemAttributes[5]),
                    itemAttributes[7].Split('~')
                );
                break;
            case "PRIORITY":
                item = new PriorityItem
                (
                    itemAttributes[0],
                    itemAttributes[1],
                    itemAttributes[2].Replace('~', ','),
                    ItemType.MEDICAL,
                    int.Parse(itemAttributes[4]),
                    int.Parse(itemAttributes[8])
                );
                break;
            case "STAT_CHANGING":
                item = new StatChangingItem
                (
                    itemAttributes[0],
                    itemAttributes[1],
                    itemAttributes[2].Replace('~', ','),
                    ItemType.MEDICAL,
                    int.Parse(itemAttributes[4]),
                    itemAttributes[9].Split('~'),
                    Array.ConvertAll(itemAttributes[10].Split('~'), int.Parse)
                );
                break;
            default:
                break;
        }

        return item;
    }
}