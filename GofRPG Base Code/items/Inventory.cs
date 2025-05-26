using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This is a class that allows the <c>Player</c>
/// class to have access to all of their items they 
/// collect in game, much like a bag.
///</summary>
public class Inventory
{
    public Dictionary<string, int> ItemList {get; private set;}

    public InventoryData InventoryData;

    public Inventory()
    {
        ItemList = new Dictionary<string, int>();
        InventoryData = new InventoryData();
    }

    ///<summary>
    /// Changes the value of the key (<paramref name="itemName"/>) 
    /// associated with it by the <paramref name="amount"/> variable.
    ///</summary>
    ///<param name="itemName">the name of the item.</param>
    ///<param name="amount">the amount you wish to change it by from -infinity to +infinity.</param>
    public void ChangeItemAmount(string itemName, int amount)
    {
        if(ItemList.ContainsKey(itemName))
        {
            int originalAmount = ItemList[itemName];
            ItemList[itemName] = originalAmount + amount <= 0 ? 0 : originalAmount + amount;
        }
        else if(amount > 0)
            AddItem(itemName, amount);
    }

    ///<summary>
    /// Sets the Player.Item variable to the item based
    /// off the <paramref name="itemName"/>.
    ///</summary>
    ///<param name="itemName">the name of the item.</param>
     public void EquipItem(string itemName)
    {
        Player p = Player.Instance();
        Item item = ItemMaker.Instance.GetItemBasedOnName(itemName);
        
        if(ItemList.ContainsKey(itemName))
        {
            p.UnequipItemFromPlayer();
            p.Item = item;
            ChangeItemAmount(itemName, -1);
        }
    }

    /// <summary>
    /// Adds the <paramref name="item"/> to the inventory
    /// based on the <paramref name="amount"/> specified.
    /// </summary>
    /// <param name="item">item to be imported</param>
    /// <param name="amount">number of items to import</param>
    public void AddItem(string itemName, int amount)
    {
        if(ItemList.ContainsKey(itemName))
            ChangeItemAmount(itemName, amount);
        else    
            ItemList.Add(itemName, amount);
    }

    public void UpdateInventoryData()
    {
        List<string> itemNames = new List<string>();
        List<int> itemAmounts = new List<int>();
        foreach(KeyValuePair<string, int> itemInfo in ItemList)
        {
            itemNames.Add(itemInfo.Key);
            itemAmounts.Add(itemInfo.Value);
        }

        InventoryData.ItemNames = itemNames.ToArray();
        InventoryData.ItemAmounts = itemAmounts.ToArray();
    }

    public void LoadUpdatedInventoryData()
    {
        for(int i = 0; i < InventoryData.ItemNames.Length; i++)
            AddItem
            (
                InventoryData.ItemNames[i], 
                InventoryData.ItemAmounts[i]
            );
    }
}