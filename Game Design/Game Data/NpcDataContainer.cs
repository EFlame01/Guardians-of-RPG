using System.Collections.Generic;

public class NpcDataContainer
{

    public static List<NpcData> NpcDataList = new List<NpcData>();

    public NpcData[] NpcDatas;

    public NpcDataContainer()
    {
        NpcDatas = NpcDataList.ToArray();
    }

    public static NpcData GetNpcData(string id)
    {
        if(NpcDataList.Count <= 0)
            return null;

        foreach(NpcData data in NpcDataList)
        {
            if(data.ID.Equals(id))
                return data;
        }
        return null;
    }

    public static void ClearNpcDataList()
    {
        NpcDataList.Clear();
    }

    public void LoadNpcDataIntoGame()
    {
        NpcDataList.AddRange(NpcDatas);
    }
}