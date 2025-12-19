using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// StartScene is a class that controls
/// the UI for the StartScene.
/// </summary>
public class StartScene : MonoBehaviour
{
    //Serialized variables
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private string nextSceneName;
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject signUpPrefab;
    [SerializeField] private GameObject loginPrefab;
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private GameObject creditsPrefab;
    [SerializeField] private GameObject guestPrefab;

    public static string Username;
    public static bool UpdatedUsername;

    public void Start()
    {
        playButton.interactable = GameManager.Instance.GameDataPresent();
        UpdateStartMenuPage();
    }

    public void Update()
    {
        UpdateStartMenuPage();
        playButton.interactable = GameManager.Instance.GameDataPresent();
    }

    public void OnSignUpButtonPressed()
    {
        Instantiate(signUpPrefab, null);
    }

    public void OnLoginButtonPressed()
    {
        Instantiate(loginPrefab, null);
    }

    public void OnLogoutButtonPressed()
    {
        GameManager.Instance.Logout();
        Username = null;
        UpdatedUsername = false;
    }

    public void OnOptionButtonPressed()
    {
        Instantiate(optionPrefab, null);
    }

    public void OnCreditsButtonPressed()
    {
        Instantiate(creditsPrefab, null);
    }

    public void OnPlayButtonPressed()
    {

        // SceneLoader.Instance.LoadScene(null, TransitionType.FADE_TO_BLACK);
        GameManager.Instance.LoadGame();
    }

    public void OnPlayAsGuestPressed()
    {
        Instantiate(guestPrefab, null);
    }

    private void UpdateStartMenuPage()
    {
        UpdatedUsername = false;
        if (Username != null)
        {
            usernameText.text = Username;
            logoutButton.gameObject.SetActive(true);
            loginButton.gameObject.SetActive(false);
            signUpButton.gameObject.SetActive(false);
        }
        else
        {
            usernameText.text = "[No Account]";
            logoutButton.gameObject.SetActive(false);
            loginButton.gameObject.SetActive(true);
            signUpButton.gameObject.SetActive(true);
        }
    }
}