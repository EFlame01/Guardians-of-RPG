using System;
using UnityEngine;

[Serializable]
public class DayNightCycleData
{
    public int TimeOfDay;
    public bool StartTimer;
    public int NumberOfDays;

    public DayNightCycleData()
    {
        TimeOfDay = GameManager.Instance.TimeOfDay;
        NumberOfDays = GameManager.Instance.NumberOfDays;
        StartTimer = GameManager.Instance.StartDayNightCycle;
    }

    public void LoadDayNightCycleData()
    {
        GameManager.Instance.TimeOfDay = TimeOfDay;
        GameManager.Instance.NumberOfDays = NumberOfDays;
        GameManager.Instance.StartDayNightCycle = StartTimer;
    }
}