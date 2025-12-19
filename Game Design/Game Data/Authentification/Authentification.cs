using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class Authentification : MonoBehaviour
{
    private const string SIGN_UP_SUCCESSFUL = "SignUp is successful.";
    private const string SIGN_IN_SUCCESSFUL = "SignIn is successful.";
    private const string USERNAME_EXISTS = "Username already exists.";
    private const string PLAYER_ALREADY_SIGNED_IN = "This player is already signed in.";
    private const string USERNAME_PASSWORD_DONT_MATCH = "Invalid username or password";
    private const string PASSWORDS_DONT_MATCH_REQ = "Password does not match requirements:\n- 1 uppercase\n- 1 lowercase\n- 1 digit\n- 1 symbol\n - minimum 8 characters\n- maximum 30 characters";
    private const string NETWORK_ERROR = "Internet issue.\nConnect to internet or play as guest.";
    private const string OTHER_ERROR = "";

    private async void Start()
    {
        await GameManager.Instance.CheckForInitialization();
    }

    public async Task<string> SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await GameManager.Instance.CheckForInitialization();

            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);

            Debug.Log(SIGN_UP_SUCCESSFUL);
            return SIGN_UP_SUCCESSFUL;
        }
        catch (AuthenticationException ex)
        {
            Debug.LogWarning("Authentication Exception: " + ex.Message);

            if (ex.Message.Contains("username already exists"))
                return USERNAME_EXISTS;
            if (ex.Message.Contains("The player is already signed in."))
                return PLAYER_ALREADY_SIGNED_IN;
            else
                return OTHER_ERROR;
        }
        catch (RequestFailedException ex)
        {
            Debug.LogWarning("RequestFailed Exception: " + ex.Message);

            if (ex.Message.Contains("Password does not match requirements."))
                return PASSWORDS_DONT_MATCH_REQ;
            if (ex.Message.Contains("Network Error: Cannot resolve destination host"))
                return NETWORK_ERROR;
            return ex.Message;
        }
        catch (Exception e)
        {
            Debug.LogWarning("ERROR: " + e.Message);
            return OTHER_ERROR;
        }
    }

    public async Task<string> SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await GameManager.Instance.CheckForInitialization();

            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);

            Debug.Log(SIGN_IN_SUCCESSFUL);
            return SIGN_IN_SUCCESSFUL;
        }
        catch (AuthenticationException ex)
        {
            Debug.LogWarning("Authentication Exception: " + ex.Message);
            if (ex.Message.Contains("do not match"))
                return USERNAME_PASSWORD_DONT_MATCH;

            return OTHER_ERROR;
        }
        catch (RequestFailedException ex)
        {
            Debug.LogWarning("RequestFailed Exception: " + ex.Message);

            if (ex.Message.Contains("Network Error: Cannot resolve destination host"))
                return NETWORK_ERROR;
            if (ex.Message.Contains("Invalid username or password"))
                return USERNAME_PASSWORD_DONT_MATCH;

            return OTHER_ERROR;
        }
        catch (Exception e)
        {
            Debug.Log("ERROR: " + e.Message);
            return OTHER_ERROR;
        }
    }

    public void Logout()
    {
        if (AuthenticationService.Instance.IsSignedIn)
            AuthenticationService.Instance.SignOut();
    }
}