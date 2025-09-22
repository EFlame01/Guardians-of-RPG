using System;
using UnityEngine;

/// <summary>
/// MedicalCenterData is a class
/// that compresses the meta data
/// for <c>MedicalObject</c> based
/// on the id, number of times it's 
/// been used per day, and the max
/// amount of times it can be used
/// per day.
/// </summary>
[Serializable]
public class MedicalCenterData
{
    //public variables
    public string ID;
    public int NumOfTimesUsed;
    public int Limit;

    //Constructor
    public MedicalCenterData(string id, int num, int lim)
    {
        ID = id;
        NumOfTimesUsed = num;
        Limit = lim;
    }
}