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
    //public static variables
    public static float Timer { get; private set; }
    public static float TimeRemaining { get; private set; }

    //private variables
    private float _prevTime;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.StartDayNightCycle)
            StartTimer();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (!GameManager.Instance.StartDayNightCycle)
            return;

        Timer += Time.fixedDeltaTime;
        TimeRemaining -= Time.fixedDeltaTime;

        if ((int)Timer % (int)Units.TIME_PER_PART == 0)
            ChangeTimeOfDay();
    }

    public static void SetTimeRemaining(float timeRemaining)
    {
        TimeRemaining = timeRemaining;
    }

    /// <summary>
    /// Starts timer to begin the Day/Night cycle.
    /// </summary>
    public void StartTimer()
    {
        TimeRemaining = Units.TIME_PER_PART;
        Timer = 0;
        _prevTime = 0;
        GameManager.Instance.StartDayNightCycle = true;
    }

    /// <summary>
    /// Stops the timer for the Day/Night cycle.
    /// </summary>
    public void StopTimer()
    {
        GameManager.Instance.StartDayNightCycle = false;
    }

    /// <summary>
    /// Resumes the timer for the Day/Night cycle.
    /// </summary>
    public void ResumeTimer()
    {
        GameManager.Instance.StartDayNightCycle = true;
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
        Timer = 0;
        _prevTime = 0;
        GameManager.Instance.TimeOfDay = timeOfDay == -1 ? GameManager.Instance.TimeOfDay : timeOfDay;
        GameManager.Instance.StartDayNightCycle = startTimer;
    }

    /// <summary>
    /// This method sets everything to a new day
    /// </summary>
    public void SetNewDay(int timeOfDay)
    {
        GameManager.Instance.TimeOfDay = timeOfDay == -1 ? Units.MORNING : timeOfDay;
        WellDataContainer.IncrementWellDay();
        MedicalCenterDataContainer.ResetNumOfTimesUsed();
        GameManager.Instance.NumberOfDays++;
    }

    /// <summary>
    /// Automatically changes the time of day
    /// to the next stage, and resets the time.
    /// </summary>
    private void ChangeTimeOfDay()
    {
        if (Timer - _prevTime < Units.TIME_PER_PART)
            return;

        _prevTime = Timer;
        TimeRemaining = Units.TIME_PER_PART;

        if (GameManager.Instance.TimeOfDay + 1 > Units.NIGHT)
        {
            SetNewDay(Units.MORNING);
        }
        else
            GameManager.Instance.TimeOfDay++;
    }
}
