using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPage : StartMenuPage
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TextMeshProUGUI errorMessage;
    [SerializeField] private GameObject deleteGameDataPrefab;

    public void Start()
    {
        InitLoginPage();
    }

    public void InitLoginPage()
    {
        ClearInputFields();
        errorMessage.text = "";
    }

    public async void OnLoginButtonPressed()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        string loginResult = await GameManager.Instance.SignIn(username, password);
        if (!loginResult.Equals("SignIn is successful."))
        {
            ClearInputFields();
            errorMessage.text = loginResult;
        }
        else
        {
            await GameManager.Instance.SignIn(username, password);
            GameManager.Instance.LoadGameData(username);
            SetUsernameText(username);
            OnBackButtonPressed();
        }
    }

    [System.Obsolete]
    public async void OnDeleteButtonPressed()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        string loginResult = await GameManager.Instance.SignIn(username, password);
        if (!loginResult.Equals("SignIn is successful."))
        {
            ClearInputFields();
            errorMessage.text = loginResult;
        }
        else
        {
            DeleteAccount deleteAccountPage = Instantiate(deleteGameDataPrefab, null).GetComponent<DeleteAccount>();
            deleteAccountPage.yesButton.onClick.AddListener(() =>
            {
                GameManager.Instance.DeleteGameData(username);
                ClearInputFields();
                errorMessage.text = "Successfully deleted " + username;
            });
            deleteAccountPage.noButton.onClick.AddListener(() =>
            {
                GameManager.Instance.Logout();
                errorMessage.text = "You did not delete " + username;
            });
        }
    }

    private void ClearInputFields()
    {
        usernameField.text = "";
        passwordField.text = "";
    }
}
