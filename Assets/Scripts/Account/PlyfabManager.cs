using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using System;

public class PlayfabManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_Text errorText;
    private GameManager gameManager = GameManager.Instance;

    public void Signup()
    {
        if (string.IsNullOrWhiteSpace(email.text) || string.IsNullOrWhiteSpace(password.text))
        {
            errorText.text = "Please fill all fields";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = email.text,
            Password = password.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnSignupSuccess, OnError);
    }

    public void Login()
    {
        if (string.IsNullOrWhiteSpace(email.text) || string.IsNullOrWhiteSpace(password.text))
        {
            errorText.text = "Please fill all fields";
            return;
        }

        var request = new LoginWithEmailAddressRequest {
            Email = email.text,
            Password = password.text,
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    public void RecoverPass()
    {
        if (string.IsNullOrWhiteSpace(email.text))
        {
            errorText.text = "Please fill all fields";
            return;
        }

        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email.text,
            TitleId = "13C11"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverySuccess, OnError);
    }

    void OnSignupSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Signup Successful");
        gameManager.Lobby();
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login Successful");
        gameManager.Lobby();
    }

    void OnRecoverySuccess(SendAccountRecoveryEmailResult result){
        Debug.Log("Email Sent!");
    }

    void OnError(PlayFabError error)
    {
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidParams:
                errorText.text = "PlayFab Error: Invalid parameters provided.";
                break;
            case PlayFabErrorCode.AccountNotFound:
                errorText.text = "PlayFab Error: Account not found.";
                break;
            case PlayFabErrorCode.NotAuthenticated:
                errorText.text = "PlayFab Error: User not authenticated. Please log in.";
                break;
            case PlayFabErrorCode.APIClientRequestRateLimitExceeded:
                errorText.text = "PlayFab Error: API request rate limit exceeded. Retrying with delay.";
                break;
            case PlayFabErrorCode.ServiceUnavailable:
                errorText.text = "PlayFab Error: PlayFab service unavailable. Retrying with delay.";
                break;
            default:
                errorText.text = "Unknown Error";
                break;
        }
    }
}