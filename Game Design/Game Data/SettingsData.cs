using System;

[Serializable]
public class SettingsData
{
    public bool GameSFX;
    public float GameVolume;
    public float GameTextSpeed;
    public bool EnableTouchPad;

    public SettingsData()
    {
        GameSFX = GameManager.Instance.GameSFX;
        GameVolume = GameManager.Instance.GameVolume;
        GameTextSpeed = GameManager.Instance.GameTextSpeed;
        EnableTouchPad = GameManager.Instance.EnableTouchPad;
    }

    public void LoadSettingsData()
    {
        GameManager.Instance.GameSFX = GameSFX;
        GameManager.Instance.GameVolume = GameVolume;
        GameManager.Instance.GameTextSpeed = GameTextSpeed;
        GameManager.Instance.EnableTouchPad = EnableTouchPad;
    }
}