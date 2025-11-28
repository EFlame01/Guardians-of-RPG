using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// StartScene is a class that controls
/// the UI for the StartScene.
/// </summary>
public class StartScene : MonoBehaviour
{
    //Serialized variables
    [SerializeField] private string nextSceneName;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject windowsSettingsPrefab;

    public void Start()
    {
        if (SaveSystem.LoadPlayerData() == null)
            continueButton.interactable = false;
    }

    /// <summary>
    /// Starts the game by taking you
    /// to the IntroScene.
    /// </summary>
    public void OnStartButtonPressed()
    {
        AudioManager.Instance.StopCurrentMusic(false);
        SaveSystem.DeleteSavedData();
        SceneLoader.Instance.LoadScene(nextSceneName, TransitionType.FADE_TO_BLACK);
    }

    /// <summary>
    /// Uses the GameManager to load your
    /// saved data and take you to where
    /// you left off in the game.
    /// </summary>
    public void OnContinueButtonPressed()
    {
        AudioManager.Instance.StopCurrentMusic(false);
        GameManager.LoadGame();
    }
}