using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using System;

public class PlayfabManager : MonoBehaviour
{
    // public void Start()
    // {
    //     if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)){
    //         /*
    //         Please change the titleId below to your own titleId from PlayFab Game Manager.
    //         If you have already set the value in the Editor Extensions, this can be skipped.
    //         */
    //         PlayFabSettings.staticSettings.TitleId = "13C11";
    //     }
    //     var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true};
    //     PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    // }

    // private void OnLoginSuccess(LoginResult result)
    // {
    //     Debug.Log("Congratulations, you made your first successful API call!");
    // }

    // private void OnLoginFailure(PlayFabError error)
    // {
    //     Debug.LogWarning("Something went wrong with your first API call.  :(");
    //     Debug.LogError("Here's some debug information:");
    //     Debug.LogError(error.GenerateErrorReport());
    // }

    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_Text errorText;

    public void Signup()
    {
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
        var request = new LoginWithEmailAddressRequest {
            Email = email.text,
            Password = password.text,
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    public void RecoverPass()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email.text,
            TitleId = "13C11"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverySuccess, OnError);
    }
    
    void OnSignupSuccess(RegisterPlayFabUserResult result){
        Debug.Log("Signup Successful");
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login Successful");
    }

    void OnRecoverySuccess(SendAccountRecoveryEmailResult result){
        Debug.Log("Email Sent!");
    }

    void OnError(PlayFabError error)
    {
        // switch (error.ErrorCode)
        // {
        //     case PlayFabErrorCode.InvalidParams:
        //         errorText.text = "PlayFab Error: Invalid parameters provided.";
        //         break;
        //     case PlayFabErrorCode.AccountNotFound:
        //         errorText.text = "PlayFab Error: Account not found.";
        //         break;
        //     case PlayFabErrorCode.NotAuthenticated:
        //         errorText.text = "PlayFab Error: User not authenticated. Please log in.";
        //         break;
        //     case PlayFabErrorCode.APIClientRequestRateLimitExceeded:
        //         errorText.text = "PlayFab Error: API request rate limit exceeded. Retrying with delay.";
        //         break;
        //     case PlayFabErrorCode.ServiceUnavailable:
        //         errorText.text = "PlayFab Error: PlayFab service unavailable. Retrying with delay.";
        //         break;
        //     default:
        //         errorText.text = "Unknown Error";
        //         break;
        // }
        errorText.text = error.GenerateErrorReport();
    }
}