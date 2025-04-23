using UnityEngine;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    [SerializeField] public string nextSceneName;
    [SerializeField] public Button continueButton;
    [SerializeField] public GameObject windowsSettingsPrefab;

    public void Start()
    {
        if(SaveSystem.LoadPlayerData() == null)
            continueButton.interactable = false;
    }

    public void OnStartButtonPressed()
    {
        AudioManager.Instance.StopCurrentMusic(false);
        SceneLoader.Instance.LoadScene(nextSceneName, TransitionType.FADE_TO_BLACK);
    }

    public void OnContinueButtonPressed()
    {
        GameManager.LoadGame();
    }
}