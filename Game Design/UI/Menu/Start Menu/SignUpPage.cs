using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SignUp : StartMenuPage
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField confirmPasswordField;
    [SerializeField] private TextMeshProUGUI errorMessage;

    void Start()
    {
        InitPage();
    }

    public async void OnSignUpButtonPressed()
    {
        string username = usernameField.text;
        string password = passwordField.text;

        if (!AllFieldsMarked())
        {
            errorMessage.text = "Please fill all fields.";
            return;
        }
        if (!passwordField.text.Equals(confirmPasswordField.text))
        {
            errorMessage.text = "Passwords do not match.";
            return;
        }
        string signUpResult = await GameManager.Instance.SignUp(username, password);
        if (!signUpResult.Equals("SignUp is successful."))
        {
            errorMessage.text = signUpResult;
            return;
        }

        await GameManager.Instance.SignUp(username, password);
        GameManager.Instance.CreateGameData(username, password);
        SetUsernameText(username);
        OnBackButtonPressed();
    }

    private void InitPage()
    {
        ClearFields();
        errorMessage.text = "";
    }

    private void ClearFields()
    {
        usernameField.text = "";
        // emailField.text = "";
        passwordField.text = "";
        confirmPasswordField.text = "";
    }

    private bool AllFieldsMarked()
    {
        if (usernameField.text.Length == 0)
            return false;
        // if (emailField.text.Length == 0)
        //     return false;
        if (passwordField.text.Length == 0)
            return false;
        if (confirmPasswordField.text.Length == 0)
            return false;
        return true;
    }
}
