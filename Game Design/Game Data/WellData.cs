using System;
using UnityEngine;

[Serializable]
public class WellData
{
    public string ID;
    public int DaysWithoutWater;
    public int NumberOfWater;

    public WellData(string id, int days, int num)
    {
        ID = id;
        DaysWithoutWater = days;
        NumberOfWater = num;
    }

}