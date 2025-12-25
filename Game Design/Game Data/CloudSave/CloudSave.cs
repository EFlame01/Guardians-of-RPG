using System.Collections.Generic;
using Unity.Services.CloudSave;
using UnityEngine;

public class CloudSave : MonoBehaviour
{
    public GameDataCloud GameData { get; private set; }

    private void Start()
    {
        InitGameDataCloud();
    }

    public async void InitGameDataCloud()
    {
        GameData = new GameDataCloud();
        await GameManager.Instance.CheckForInitialization();
    }

    public async void CreateData(string username, string password)
    {
        GameData.Username = username;
        GameData.Password = password;

        //after signing up with username and password, create game data
        var gameData = new Dictionary<string, object>
        {
            {GameData.Username, GameData.SaveGameData()}
        };

        //after creating game data, add data to cloud
        await CloudSaveService.Instance.Data.Player.SaveAsync(gameData);
    }

    public async void SaveData()
    {
        var gameData = new Dictionary<string, object>
        {
            {GameData.Username, GameData.SaveGameData()}
        };
        await CloudSaveService.Instance.Data.Player.SaveAsync(gameData);
        Debug.Log("Saved data...");
    }

    public async void LoadData(string username)
    {
        var gameData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { username });

        if (gameData.TryGetValue(username, out var keyName))
        {
            string json = keyName.Value.GetAsString();
            GameData = JsonUtility.FromJson<GameDataCloud>(json);
        }
    }
}