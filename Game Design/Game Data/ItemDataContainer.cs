using System.Collections.Generic;

public class ItemDataContainer
{

    public static List<ItemData> ItemDataList = new List<ItemData>();

    public ItemData[] ItemDatas;

    public ItemDataContainer()
    {
        ItemDatas = ItemDataList.ToArray();
    }

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

    public static void ClearItemDataList()
    {
        ItemDataList.Clear();
    }

    public void LoadItemDataIntoGame()
    {
        ItemDataList.AddRange(ItemDatas);
    }
}