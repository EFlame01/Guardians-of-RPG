using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameDataCloud
{
    //General Game Data
    public string Username;
    public string Password;
    public PlayerDataCloud PlayerData;

    //Game Settings Data
    public bool GameSFX;
    public float GameVolume;
    public float GameTextSpeed;
    public bool EnableTouchPad;

    public GameDataCloud()
    {

    }

    public void UpdatePlayerData(PlayerDataCloud playerData)
    {
        PlayerData = playerData;
    }

    public GameDataCloud SaveGameData()
    {
        //Updating Player Data
        Debug.Log("Saving Player Data");

        PlayerDataCloud playerData = new PlayerDataCloud();
        playerData.PlayerSceneName = SceneManager.GetActiveScene().name;
        TimeTracker.Instance().EndTime();
        playerData.TotalSavedPlayTime = TimeTracker.Instance().TotalSavedPlayTime;
        UpdatePlayerData(playerData);

        //Updating Game Data
        GameSFX = GameManager.Instance.GameSFX;
        GameVolume = GameManager.Instance.GameVolume;
        GameTextSpeed = GameManager.Instance.GameTextSpeed;
        EnableTouchPad = GameManager.Instance.EnableTouchPad;

        //returning data in form of GameDataCloud object
        return this;
    }
}