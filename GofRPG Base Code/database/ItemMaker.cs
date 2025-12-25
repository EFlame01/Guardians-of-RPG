using System;

/// <summary>
/// ItemMaker is a class that parses through
/// the data to create <c>Item</c> objects for
/// the <c>Character</c> class.
/// </summary>
public class ItemMaker : Singleton<ItemMaker>
{
    private const int ITEM_INDEX = 6;

    /// <summary>
    /// Gets and returns the <c>Item</c> object based on the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the object</param>
    /// <returns>the <c>Item</c> objects or <c>null</c> if the item could not be found.</returns>
    public Item GetItemBasedOnName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        string[] itemAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[ITEM_INDEX], name).Split(',');

        return itemAttributes[3] switch
        {
            "FOOD" => new FoodItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.FOOD,
                                int.Parse(itemAttributes[4]),
                                int.Parse(itemAttributes[11]),
                                int.Parse(itemAttributes[5])
                            ),
            "HEALING" => new HealingItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.HEALING,
                                int.Parse(itemAttributes[4]),
                                int.Parse(itemAttributes[11]),
                                int.Parse(itemAttributes[6])
                            ),
            "KEY" => new KeyItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.KEY,
                                int.Parse(itemAttributes[4])
                            ),
            "MEDICAL" => new MedicalItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.MEDICAL,
                                int.Parse(itemAttributes[4]),
                                int.Parse(itemAttributes[11]),
                                int.Parse(itemAttributes[5]),
                                itemAttributes[7].Split('~')
                            ),
            "PRIORITY" => new PriorityItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.PRIORITY,
                                int.Parse(itemAttributes[4]),
                                int.Parse(itemAttributes[11]),
                                int.Parse(itemAttributes[8])
                            ),
            "STAT_CHANGING" => new StatChangingItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.STAT_CHANGING,
                                int.Parse(itemAttributes[4]),
                                int.Parse(itemAttributes[11]),
                                itemAttributes[9].Split('~'),
                                Array.ConvertAll(itemAttributes[10].Split('~'), int.Parse)
                            ),
            _ => null,
        };
    }
}