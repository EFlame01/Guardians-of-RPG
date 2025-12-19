using System.Threading.Tasks;
using Unity.Services.Core;

public class GameManager : PersistentSingleton<GameManager>
{
    public PlayerState PlayerState = PlayerState.NOT_MOVING;
    public float PlayerSpeed = Units.PLAYER_SPEED;

    public bool GameSFX = true;
    public float GameVolume = Units.MAX_VOLUME;
    public float GameTextSpeed = Units.MAX_TEXT_SPEED;

    public bool EnableKeyboardInputs = true;
    public bool EnableTouchPad = false;

    public bool EnableNarrationInputs = true;
    public bool EnableButtons = true;

    public string Leaning = "FEMALE";

    public int NumberOfDays = 0;
    public bool StartDayNightCycle = false;
    public int TimeOfDay = 0;

    private bool IsUnityServicesInitialized;

    protected override void Awake()
    {
        base.Awake();
        SaveSystem.LoadSettingsData();
    }

    private void Update()
    {
        CheckToEnableButtons();
    }

    public void LoadGame()
    {
        GameDataCloud data = GetComponentInChildren<CloudSave>().GameData;
        data.PlayerData.LoadPlayerData();
        string sceneName = data.PlayerData.PlayerSceneName.Equals("Start Scene") ? "Intro" : data.PlayerData.PlayerSceneName;
        SceneLoader.Instance.LoadScene(sceneName, TransitionType.FADE_TO_BLACK);
    }

    public void StartGame()
    {
        GetComponentInChildren<CloudSave>().InitGameDataCloud();
        Player.Instance().CreateNewInstance();
        SceneLoader.Instance.LoadScene("Intro", TransitionType.FADE_TO_BLACK);
    }

    public void CreateGameData(string username, string password)
    {
        GetComponentInChildren<CloudSave>().CreateData(username, password);
    }

    public void SaveGameData()
    {
        GetComponentInChildren<CloudSave>().SaveData();
    }

    public void LoadGameData(string username)
    {
        GetComponentInChildren<CloudSave>().LoadData(username);
    }

    public async Task<string> SignIn(string username, string password)
    {
        return await GetComponentInChildren<Authentification>().SignInWithUsernamePasswordAsync(username, password);
    }

    public async Task<string> SignUp(string username, string password)
    {
        return await GetComponentInChildren<Authentification>().SignUpWithUsernamePasswordAsync(username, password);
    }

    public void Logout()
    {
        GetComponentInChildren<Authentification>().Logout();
        GetComponentInChildren<CloudSave>().InitGameDataCloud();
    }

    public async Task CheckForInitialization()
    {
        if (!IsUnityServicesInitialized)
        {
            await UnityServices.InitializeAsync();
            IsUnityServicesInitialized = true;
        }
    }

    public bool GameDataPresent()
    {
        return GetComponentInChildren<CloudSave>().GameData.Username != null;
    }

    public bool IsPlayingAsUser()
    {
        GameDataCloud data = GetComponentInChildren<CloudSave>().GameData;
        // return data.Username != null;
        if (data.Username is null)
            return false;
        else
            return true;
    }

    // public double GetPlayTime()
    // {
    //     GameDataCloud data = GetComponentInChildren<CloudSave>().GameData;
    //     return data.PlayerData.TotalSavedPlayTime;
    // }

    private void CheckToEnableButtons()
    {
        EnableButtons = Instance.PlayerState switch
        {
            PlayerState.MOVING => true,
            PlayerState.NOT_MOVING => true,
            PlayerState.PAUSED => false,
            PlayerState.CANNOT_MOVE => false,
            PlayerState.CUT_SCENE => false,
            PlayerState.TRANSITION => false,
            PlayerState.INTERACTING_WITH_OBJECT => false,
            _ => false,
        };
    }
}
