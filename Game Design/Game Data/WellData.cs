using System;
using UnityEngine;

///<summary>
/// WellData is the class that
/// compresses the meta data
/// for <c>WellObject</c> based
/// on the id, days without water
/// and number of jugged water they
/// can have.
/// </summary>
[Serializable]
public class WellData
{
    //public variables
    public string ID;
    public int DaysWithoutWater;
    public int NumberOfWater;

    //Constructor
    public WellData(string id, int days, int num)
    {
        ID = id;
        DaysWithoutWater = days;
        NumberOfWater = num;

        WellDataContainer.WellDataList.Add(this);
    }

}