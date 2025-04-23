using System;

[Serializable]
public class ItemData
{
    public string ID;
    public bool Opened;

    public ItemData(string id, bool opened)
    {
        ID = id;
        Opened = opened;

        ItemDataContainer.ItemDataList.Add(this);
    }

    public void UpdateItemData(bool opened)
    {
        foreach(ItemData itemData in ItemDataContainer.ItemDataList)
        {
            if(itemData.ID.Equals(ID))
                itemData.Opened = opened;
        }
    }
}