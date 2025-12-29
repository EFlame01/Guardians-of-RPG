using System;

/// <summary>
/// ItemData is a class that compresses
/// the <c>ItemObject</c> class by it's
/// id and whether or not it has been opened.
/// </summary>
[Serializable]
public class ItemData
{
    //public variables
    public string ID;
    public bool Opened;

    //Constructor
    public ItemData(string id, bool opened)
    {
        ID = id;
        Opened = opened;

        ItemDataContainer.ItemDataList.Add(this);
    }

    /// <summary>
    /// Updates a specific 
    /// <c>ItemObject</c> in the 
    /// list and updates that it has
    /// been opened.
    /// </summary>
    /// <param name=""></param>
    public void UpdateItemData(bool opened)
    {
        foreach(ItemData itemData in ItemDataContainer.ItemDataList)
        {
            if(itemData.ID.Equals(ID))
                itemData.Opened = opened;
        }
    }
}