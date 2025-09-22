using System.Collections.Generic;

/// <summary>
/// ItemDataContainer is a class that keeps all
/// of the <c>ItemData</c> inside of a list
/// to be loaded and saved in the game.
/// </summary>
public class ItemDataContainer
{
    //public static variable
    public static List<ItemData> ItemDataList = new List<ItemData>();

    //public variable
    public ItemData[] ItemDatas;

    //Constructor
    public ItemDataContainer()
    {
        ItemDatas = ItemDataList.ToArray();
    }

    /// <summary>
    /// returns the <c>ItemData</c> for an
    /// <c>ItemObject</c> based on their
    /// id.
    /// </summary>
    /// <param name="id">The identifying string connecting the <c>ItemData</c> to the <c>ItemObject</c></param>
    public static ItemData GetItemData(string id)
    {
        if(ItemDataList.Count <= 0)
            return null;

        foreach(ItemData data in ItemDataList)
        {
            if(data.ID.Equals(id))
                return data;
        }
        return null;
    }

    /// <summary>
    /// Clears the entire ItemDataContainer
    /// </summary>
    public static void ClearItemDataList()
    {
        ItemDataList.Clear();
    }

    /// <summary>
    /// Loads ItemData retrieved into the
    /// ItemDataList for easy access.
    /// </summary>
    public void LoadItemDataIntoGame()
    {
        ItemDataList.AddRange(ItemDatas);
    }
}