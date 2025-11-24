using System.Collections.Generic;

/// <summary>
/// NpcDataContainer is a class
/// that keeps all the meta data for each
/// <c>NpcData</c> to be loaded
/// and saved in the game.
/// <summary>
public class NpcDataContainer
{
    //public static variables
    public static List<NpcData> NpcDataList = new List<NpcData>();

    //public variables
    public NpcData[] NpcDatas;

    //Constructor
    public NpcDataContainer()
    {
        NpcDatas = NpcDataList.ToArray();
    }

    /// <summary>
    /// returns the <c>NpcData</c> for an
    /// <c>NPCObject</c> based on their id.
    /// </summary>
    /// <param name="id">The identifying string connecting the <c>NpcData</c> to the <c>NPCObject</c>
    /// </param>
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

    /// <summary>
    /// Clears the entire NpcDataList
    /// </summary>
    public static void ClearNpcDataList()
    {
        NpcDataList.Clear();
    }

    public static void AddNpcData(NpcData data)
    {
        if(NpcDataList.Count == 0)
        {
            NpcDataList.Add(data);
            return;
        }

        for(int i = 0; i < NpcDataList.Count; i++)
        {
            if(NpcDataList[i].ID == data.ID)
            {
                NpcDataList[i] = data;
                return;
            }
        }

        NpcDataList.Add(data);
    }

    /// <summary>
    /// Loads NpcData retrieved into the
    /// NpcDataList for easy access.
    /// </summary>
    public void LoadNpcDataIntoGame()
    {
        NpcDataList.AddRange(NpcDatas);
    }
}