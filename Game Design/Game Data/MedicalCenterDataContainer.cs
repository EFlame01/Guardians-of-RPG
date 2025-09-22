using System.Collections.Generic;

/// <summary>
/// MedicalCenterDataContainer is a class
/// that keeps all the meta data for each
/// <c>MedicalCenterData</c> to be loaded
/// and saved in the game.
/// <summary>
public class MedicalCenterDataContainer
{

    //public static variable
    public static List<MedicalCenterData> MedicalCenterDataList = new List<MedicalCenterData>();

    //public variable
    public MedicalCenterData[] MedicalCenterDatas;

    //Constructor
    public MedicalCenterDataContainer()
    {
        MedicalCenterDatas = MedicalCenterDataList.ToArray();
    }

    /// <summary>
    /// returns the <c>MedicalCenterData</c> for an
    /// <c>MedicalObject</c> based on their id.
    /// </summary>
    /// <param name="id">The identifying string connecting the <c>MedicalCenterData</c> to the <c>MedicalObject</c></param>
    public static MedicalCenterData GetMedicalCenterData(string id)
    {
        if(MedicalCenterDataList.Count <= 0)
            return null;

        foreach(MedicalCenterData data in MedicalCenterDataList)
        {
            if(data.ID.Equals(id))
                return data;
        }
        return null;
    }

    ///<summary>
    /// Clears the entire MedicalDataCenterList.
    /// </summary>
    public static void ClearMedicalCenterDataList()
    {
        MedicalCenterDataList.Clear();
    }

    /// <summary>
    /// Loads MedicalCenterData retrieved into the
    /// MedicalCenterDataList for easy access.
    /// </summary>
    public void LoadMedicalCenterDataIntoGame()
    {
        MedicalCenterDataList.AddRange(MedicalCenterDatas);
    }

    /// <summary>
    /// Resets all of the <c>MedicalCenterData</c> variable
    /// NumOfTimesUsed to 0.
    /// </summary>
    public static void ResetNumOfTimesUsed()
    {
        foreach(MedicalCenterData data in MedicalCenterDataList)
        {
            data.NumOfTimesUsed = 0;
        }
    }
}