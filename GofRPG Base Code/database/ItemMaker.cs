using System;
using UnityEngine;

/// <summary>
/// ItemMaker is a class that parses through
/// the data to create <c>Item</c> objects for
/// the <c>Character</c> class.
/// </summary>
public class ItemMaker : Singleton<ItemMaker>
{
    private const int ITEM_INDEX = 6;
    private const int ITEM_DATA_LENGTH = 12;
    public static Item UNKOWN_ITEM = new KeyItem("Unknown Item", "Unknown Items", "This is the default item given to players if a certain item cannot be found in the database. If you see this, please report the bug.", ItemType.KEY, 0);


    /// <summary>
    /// Gets and returns the <c>Item</c> object based on the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the object</param>
    /// <returns>the <c>Item</c> objects or <c>null</c> if the item could not be found.</returns>
    public Item GetItemBasedOnName(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            // Debug.LogWarning($"WARNING: The item [ {name} ] is either null or empty.");
            return null;
        }

        string itemData = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[ITEM_INDEX], name);

        if (string.IsNullOrEmpty(itemData) || itemData.Split(',').Length != ITEM_DATA_LENGTH)
        {
            Debug.LogWarning($"WARNING: The item [ {name} ] cannot be found in the database. If you see this, please report this bug.");
            return UNKOWN_ITEM;
        }

        string[] itemAttributes = itemData.Split(',');

        return itemAttributes[3] switch
        {
            "FOOD" => new FoodItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.FOOD,
                                int.TryParse(itemAttributes[4], out int foodItemLevel) ? foodItemLevel : 0,
                                int.TryParse(itemAttributes[11], out int foodItemPrices) ? foodItemPrices : 100,
                                int.TryParse(itemAttributes[5], out int foodItemHealAmount) ? foodItemHealAmount : 10
                            ),
            "HEALING" => new HealingItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.HEALING,
                                int.TryParse(itemAttributes[4], out int healingItemLevel) ? healingItemLevel : 0,
                                int.TryParse(itemAttributes[11], out int healingItemPrices) ? healingItemPrices : 100,
                                double.TryParse(itemAttributes[6], out double healingItemHealPercent) ? healingItemHealPercent : 0.10
                            ),
            "KEY" => new KeyItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.KEY,
                                int.TryParse(itemAttributes[4], out int keyItemLevel) ? keyItemLevel : 0
                            ),
            "MEDICAL" => new MedicalItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.MEDICAL,
                                int.TryParse(itemAttributes[4], out int medicalItemLevel) ? medicalItemLevel : 0,
                                int.TryParse(itemAttributes[11], out int medicalItemPrices) ? medicalItemPrices : 100,
                                int.TryParse(itemAttributes[5], out int medicalItemHealAmount) ? medicalItemHealAmount : 10,
                                itemAttributes[7].Split('~')
                            ),
            "PRIORITY" => new PriorityItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.PRIORITY,
                                int.TryParse(itemAttributes[4], out int priorityItemLevel) ? priorityItemLevel : 3,
                                int.TryParse(itemAttributes[11], out int priorityItemPrices) ? priorityItemPrices : 100,
                                int.TryParse(itemAttributes[8], out int priorityItemPriorityStage) ? priorityItemPriorityStage : 3
                            ),
            "STAT_CHANGING" => new StatChangingItem
                            (
                                itemAttributes[0],
                                itemAttributes[1],
                                itemAttributes[2].Replace('~', ','),
                                ItemType.STAT_CHANGING,
                                int.TryParse(itemAttributes[4], out int statChangingItemLevel) ? statChangingItemLevel : 0,
                                int.TryParse(itemAttributes[11], out int statChangingItemPrices) ? statChangingItemPrices : 100,
                                itemAttributes[9].Split('~'),
                                Array.ConvertAll(itemAttributes[10].Split('~'), int.Parse)
                            ),
            _ => null,
        };
    }
}