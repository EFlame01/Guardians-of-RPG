using System;
using UnityEngine;

public class TimeTracker
{
    private static TimeTracker timeTracker;
    private float _sessionStartTime;
    public double TotalSavedPlayTime { get; private set; }

    public TimeTracker()
    {

    }

    public static TimeTracker Instance()
    {
        timeTracker = timeTracker == null ? new TimeTracker() : timeTracker;
        return timeTracker;
    }

    public void SetTotalSavedPlayTime(double totalSavedPlayTime)
    {
        TotalSavedPlayTime = totalSavedPlayTime;
    }

    public void StartTime()
    {
        _sessionStartTime = Time.realtimeSinceStartup;
        Debug.Log("Starting Time\n" + TotalSavedPlayTime);
    }

    public string GetPlayTime()
    {
        Debug.Log("Getting Play Time\n" + TotalSavedPlayTime);
        TimeSpan timeSpan = TimeSpan.FromSeconds(TotalSavedPlayTime);
        string time = string.Format("{0:00}:{1:00}:{2:00}", (int)timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds);
        return time;
    }

    public void EndTime()
    {
        Debug.Log("Ending Time");
        double currentSessionDuration = Time.realtimeSinceStartup - _sessionStartTime;
        _sessionStartTime = Time.realtimeSinceStartup;
        TotalSavedPlayTime += currentSessionDuration;
        GetPlayTime();
    }
}