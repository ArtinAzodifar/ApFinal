using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using System;

public class PlayfabManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_Text resultText;
    private GameManager gameManager = GameManager.Instance;

    //button methods
    public void Signup()
    {
        if (string.IsNullOrWhiteSpace(email.text) || string.IsNullOrWhiteSpace(password.text))
        {
            resultText.text = "Please fill all fields";
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
            resultText.text = "Please fill all fields";
            return;
        }

        var request = new LoginWithEmailAddressRequest
        {
            Email = email.text,
            Password = password.text,
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }
    public void RecoverPass()
    {
        if (string.IsNullOrWhiteSpace(email.text))
        {
            resultText.text = "Please fill all fields!";
            return;
        }

        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email.text,
            TitleId = "13C11"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverySuccess, OnError);
    }

    //result methods
    void OnSignupSuccess(RegisterPlayFabUserResult result)
    {
        resultText.text = "Signup Successful!";
        EmailStore.Instance.SetEmail(email.text);
        gameManager.Lobby();
    }
    private void OnLoginSuccess(LoginResult result)
    {
        resultText.text = "Login Successful!";
        EmailStore.Instance.SetEmail(email.text);
        gameManager.Lobby();
    }
    void OnRecoverySuccess(SendAccountRecoveryEmailResult result)
    {
        resultText.text = "Recovery email sent!\n please check your email.";
    }
    void OnError(PlayFabError error)
    {
        string message;
        switch (error.Error)
        {
            case PlayFabErrorCode.InvalidParams:
                message = "Please check your input. Some fields might be missing or incorrect.";
                break;
            case PlayFabErrorCode.InvalidEmailAddress:
                message = "Invalid email format.";
                break;
            case PlayFabErrorCode.EmailAddressNotAvailable:
                message = "This email is already registered.";
                break;
            case PlayFabErrorCode.AccountNotFound:
            case PlayFabErrorCode.InvalidEmailOrPassword:
                message = "Email or password is incorrect.";
                break;
            case PlayFabErrorCode.InvalidPassword:
                message = "Password is too weak or incorrect.";
                break;
            case PlayFabErrorCode.ServiceUnavailable:
                message = "PlayFab service is currently unavailable. try again later.";
                break;
            case PlayFabErrorCode.APIClientRequestRateLimitExceeded:
                message = "Too many requests. try again later.";
                break;
            default:
                message = "unknown error:\n" + error.GenerateErrorReport();
                break;
        }
        resultText.text = message;
    }

    public string getEmail()
    {
        return email.text;
    }


    //mode setting (TODO)
    public void recoverPassMode() { }
    public void signupMode() { }
    public void loginMode() { }

}