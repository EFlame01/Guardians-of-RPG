using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// LightCycle is a class that is responsible 
/// for changing the global light color value
/// based on calculations from the <c>DayNightCycle<c>
/// class and the the time of day color values.
/// </summary>
public class LightCycle : MonoBehaviour
{
    //Serialized variables
    public Light2D Light;
    public bool SetTimer;

    //private variables
    private Color MORNING_COLOR = new Color((float)(251f / 255f), (float)(222f / 255f), (float)(188f / 255f));
    private Color EVENING_COLOR = new Color((float)(207f / 255f), (float)(134f / 255f), (float)(83f / 255f));
    private Color NIGHT_COLOR = new Color((float)(59f / 255f), (float)(99f / 255f), (float)(113f / 255f));

    private void Start()
    {
        SetLight();
    }

    // Update is called once per frame
    private void Update()
    {
        if (SetTimer)
            SetLight();
    }

    /// <summary>
    /// Adjusts the global light of the scene depending on the 
    /// time of day it is. The time of day is calculated
    /// by dividing the TimeRemaining from the
    /// <c>DayNightCycle</c> class and TIME_PER_PART from
    /// the <c>Units</c> class.
    /// </summary>
    private void SetLight()
    {
        Light.color = DayNightCycle.TimeOfDay switch
        {
            Units.MORNING => Color.Lerp(EVENING_COLOR, MORNING_COLOR, (float)DayNightCycle.TimeRemaining / Units.TIME_PER_PART),//use day light
            Units.EVENING => Color.Lerp(NIGHT_COLOR, EVENING_COLOR, (float)DayNightCycle.TimeRemaining / Units.TIME_PER_PART),//use afternoon light
            Units.NIGHT => Color.Lerp(MORNING_COLOR, NIGHT_COLOR, (float)DayNightCycle.TimeRemaining / Units.TIME_PER_PART),//use night light
            _ => MORNING_COLOR,//use day light
        };
    }
}