using System;

/// <summary>
/// SettingsData is the class
/// that compresses the meta data
/// for the Settings to be loaded in 
/// later.
/// </summary>
[Serializable]
public class SettingsData
{
    //public variables
    public bool GameSFX;
    public float GameVolume;
    public float GameTextSpeed;
    public bool EnableTouchPad;

    //Constructor
    public SettingsData()
    {
        GameSFX = GameManager.Instance.GameSFX;
        GameVolume = GameManager.Instance.GameVolume;
        GameTextSpeed = GameManager.Instance.GameTextSpeed;
        EnableTouchPad = GameManager.Instance.EnableTouchPad;
    }

    /// <summary>
    /// Takes the loaded saved data and adjust
    /// the game settings based off that.
    /// </summary>
    public void LoadSettingsData()
    {
        GameManager.Instance.GameSFX = GameSFX;
        GameManager.Instance.GameVolume = GameVolume;
        GameManager.Instance.GameTextSpeed = GameTextSpeed;
        GameManager.Instance.EnableTouchPad = EnableTouchPad;
    }
}