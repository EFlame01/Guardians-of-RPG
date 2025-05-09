using System.Collections.Generic;

public class MedicalCenterDataContainer
{

    public static List<MedicalCenterData> MedicalCenterDataList = new List<MedicalCenterData>();

    public MedicalCenterData[] MedicalCenterDatas;

    public MedicalCenterDataContainer()
    {
        MedicalCenterDatas = MedicalCenterDataList.ToArray();
    }

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

    public static void ClearMedicalCenterDataList()
    {
        MedicalCenterDataList.Clear();
    }

    public void LoadMedicalCenterDataIntoGame()
    {
        MedicalCenterDataList.AddRange(MedicalCenterDatas);
    }

    public static void ResetNumOfTimesUsed()
    {
        foreach(MedicalCenterData data in MedicalCenterDataList)
        {
            data.NumOfTimesUsed = 0;
        }
    }
}