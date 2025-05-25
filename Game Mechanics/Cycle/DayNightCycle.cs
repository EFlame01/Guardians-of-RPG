using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

/// <summary>
/// DayNightCycle is a PersistentSingleton class that is responsible
/// for setting the timing in the game. By setting the timing, it allows
/// the game to set up their day night cycle.
/// </summary>
public class DayNightCycle : PersistentSingleton<DayNightCycle>
{

    public static int TimeOfDay {get; private set;}
    public static float Timer {get; private set;}
    public static float TimeRemaining {get; private set;}
    private bool _timerStarted = false;
    private float _prevTime;

    // Start is called before the first frame update
    void Start()
    {
        StartTimer();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if(_timerStarted)
        {
            Debug.Log("Day Night Cycle Active...");
            Timer += Time.fixedDeltaTime;
            TimeRemaining -= Time.fixedDeltaTime;
        }
        Debug.Log("Day Night Cycle NOT Active...");

        if((int)Timer%(int)Units.TIME_PER_PART == 0)
            ChangeTimeOfDay();
    }

    /// <summary>
    /// Starts timer to begin the Day/Night cycle.
    /// </summary>
    public void StartTimer()
    {
        TimeRemaining = Units.TIME_PER_PART;
        TimeOfDay = 0;
        Timer = 0;
        _prevTime = 0;
        _timerStarted = true;
    }

    /// <summary>
    /// Stops the timer for the Day/Night cycle.
    /// </summary>
    public void StopTimer()
    {
        _timerStarted = false;
    }

    public void ResumeTimer()
    {
        _timerStarted = true;
    }

    /// <summary>
    /// Sets the Time of Day with <paramref name="timeOfDay"/>
    /// and resets the timer.
    /// </summary>
    /// <param name="timeOfDay">Determines what time of day to set the day</param>
    /// <param name="startTimer">Determines if the timer should start</param>
    public void SetTimeOfDay(int timeOfDay, bool startTimer)
    {
        TimeRemaining = Units.TIME_PER_PART;
        TimeOfDay = timeOfDay;
        Timer = 0;
        _prevTime = 0;
        _timerStarted = startTimer;
    }

    /// <summary>
    /// Automatically changes the time of day
    /// to the next stage, and resets the time.
    /// </summary>
    private void ChangeTimeOfDay()
    {
        if(Timer - _prevTime < Units.TIME_PER_PART)
            return;
        
        _prevTime = Timer;
        TimeRemaining = Units.TIME_PER_PART;

        if(TimeOfDay + 1 > Units.NIGHT)
        {
            TimeOfDay = Units.MORNING;
            WellDataContainer.IncrementWellDay();
            MedicalCenterDataContainer.ResetNumOfTimesUsed();
            GameManager.Instance.NumberOfDays++;
        }
        else
            TimeOfDay++;
    }
}
