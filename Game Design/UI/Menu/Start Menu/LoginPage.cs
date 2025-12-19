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

        // bool validCredentials = await GameManager.Instance.CheckLoginCredentials(username, password);
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

    private void ClearInputFields()
    {
        usernameField.text = "";
        passwordField.text = "";
    }
}
