
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

    protected override void Awake()
    {
        base.Awake();
        SaveSystem.LoadSettingsData();
    }

    private void Update()
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

    public static void SaveGame()
    {
        SaveSystem.DeleteSavedData();
        SaveSystem.SaveSettingsData();
        SaveSystem.SavePlayerData();
        SaveSystem.SaveInventoryData();
        SaveSystem.SaveItemData();
        SaveSystem.SaveNpcData();
        SaveSystem.SaveQuestData();
        SaveSystem.SaveStoryFlagData();
        SaveSystem.SaveWellData();
        SaveSystem.SaveMedicalCenterData();
    }

    public static void LoadGame()
    {
        PlayerData playerData = SaveSystem.LoadPlayerData();
        SaveSystem.LoadInventoryData();
        SaveSystem.LoadItemData();
        SaveSystem.LoadNpcData();
        SaveSystem.LoadQuestData();
        SaveSystem.LoadStoryFlagData();
        SaveSystem.LoadWellData();
        SaveSystem.LoadMedicalCenterData();

        SceneLoader.Instance.LoadScene(playerData.sceneName, TransitionType.FADE_TO_BLACK);
    }
}
