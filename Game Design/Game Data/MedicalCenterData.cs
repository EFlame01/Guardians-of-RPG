using System;
using UnityEngine;

[Serializable]
public class MedicalCenterData
{
    public string ID;
    public int NumOfTimesUsed;
    public int Limit;

    public MedicalCenterData(string id, int num, int lim)
    {
        ID = id;
        NumOfTimesUsed = num;
        Limit = lim;
    }

}