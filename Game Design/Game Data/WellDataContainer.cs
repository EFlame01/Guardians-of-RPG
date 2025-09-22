using System.Collections.Generic;

/// <summary>
/// WellDataContainer is a class
/// that keeps all the meta data for each
/// <c>WellData</c> to be loaded
/// and saved in the game.
/// <summary>
public class WellDataContainer
{
    //public static variable
    public static List<WellData> WellDataList = new List<WellData>();

    //public variable
    public WellData[] WellDatas;

    //Constructor
    public WellDataContainer()
    {
        WellDatas = WellDataList.ToArray();
    }

    /// <summary>
    /// returns the <c>WellData</c> for an
    /// <c>WellObject</c> based on their id.
    /// </summary>
    /// <param name="id">The identifying string connecting the <c>WellData</c> to the <c>WellObject</c>
    /// </param>
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

    /// <summary>
    /// Clears the entire NpcDataList
    /// </summary>
    public static void ClearWellDataList()
    {
        WellDataList.Clear();
    }

    /// <summary>
    /// Loads WellData retrieved into the
    /// WellDataList for easy access.
    /// </summary>
    public void LoadWellDataIntoGame()
    {
        WellDataList.AddRange(WellDatas);
    }

    /// <summary>
    /// Resets all of the <c>WellData</c> variable
    /// DaysWithoutWater to 0.
    /// </summary>
    public static void IncrementWellDay()
    {
        foreach(WellData data in WellDataList)
        {
            data.DaysWithoutWater++;
        }
    }
}