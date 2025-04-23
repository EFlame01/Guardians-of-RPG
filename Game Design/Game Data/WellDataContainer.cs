using System.Collections.Generic;

public class WellDataContainer
{

    public static List<WellData> WellDataList = new List<WellData>();

    public WellData[] WellDatas;

    public WellDataContainer()
    {
        WellDatas = WellDataList.ToArray();
    }

    public static WellData GetWellData(string id)
    {
        if(WellDataList.Count <= 0)
            return null;

        foreach(WellData data in WellDataList)
        {
            if(data.ID.Equals(id))
                return data;
        }
        return null;
    }

    public static void ClearWellDataList()
    {
        WellDataList.Clear();
    }

    public void LoadWellDataIntoGame()
    {
        WellDataList.AddRange(WellDatas);
    }

    public static void IncrementWellDay()
    {
        foreach(WellData data in WellDataList)
        {
            data.DaysWithoutWater++;
        }
    }
}