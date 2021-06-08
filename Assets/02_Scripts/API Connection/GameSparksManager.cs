using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameSparksManager : MonoBehaviour
    {
    public GameObject GameSparksPanel;
    public GameObject PhotonPanel;

    [HeaderAttribute("Register Form")]
    public InputField RegisterUsername;
    public InputField RegisterPassword;
    public InputField RegisterPasswordVerify;
    

    [HeaderAttribute("Login Form")]
    public InputField LoginUsername;
    public InputField LoginPassword;


    [HeaderAttribute("Error")]
    public AudioClip ErrorSFX;
    public Text LoginErrorText;
    public Text RegisterErrorText;

    public void RegisterButton()
        {
        if (RegisterUsername.text != "" && RegisterPassword.text != "" && RegisterPasswordVerify.text != "")
            {
            if (RegisterPassword.text == RegisterPasswordVerify.text)
                {
                Debug.Log("Try to Register " + RegisterUsername.text);
                RegisterGameSparksPlayer(RegisterUsername.text, RegisterPassword.text);
                }
            else
                {
                RegisterError("VERIFY PASSWORD IS DIFFERENT FROM PASSWORD");
                }
            }
        else
            {
            RegisterError("USERNAME, PASSWORD OR VERIFY PASSWORD IS EMPTY");
            }
        }

    public void RegisterError(string Error)
        {
        Debug.Log(Error);
        RegisterErrorText.gameObject.SetActive(true);
        RegisterErrorText.text = Error;
        Invoke("MakeInvisible", 5);
        }

    public void LoginButton()
        {
        if (LoginUsername.text != "" && LoginPassword.text != "")
            {
            Debug.Log("Try to Login " + LoginUsername.text);
            LoginGameSparksPlayer(LoginUsername.text, LoginPassword.text);
            }
        else
            {
            LoginError("USERNAME OR PASSWORD IS EMPTY");
            }
        }

    public void LoginError(string Error)
        {
        Debug.Log(Error);
        LoginErrorText.gameObject.SetActive(true);
        LoginErrorText.text = Error;
        Invoke("MakeInvisible", 5);
        }

    private void MakeInvisible()
        {
        RegisterErrorText.gameObject.SetActive(false);
        LoginErrorText.gameObject.SetActive(false);
        }

    private void RegisterGameSparksPlayer(string Username, string Password)
        {

        new GameSparks.Api.Requests.RegistrationRequest()
  .SetDisplayName(Username)
  .SetPassword(Password)
  .SetUserName(Username)
  .Send((response) => {
      if (!response.HasErrors)
          {
          Debug.Log("Player Registered");
          LoginGameSparksPlayer(Username, Password);
          }
      else
          {
          Debug.Log("Error Registering Player");
          RegisterError(response.Errors.JSON);
          SFXManager.Instance.MainSFX.PlayOneShot(ErrorSFX);
          }
  }
);
        }

    private void LoginGameSparksPlayer(string Username, string Password)
        {
        new GameSparks.Api.Requests.AuthenticationRequest().SetUserName(Username).SetPassword(Password).Send((response) => {
            if (!response.HasErrors)
                {
                Debug.Log("Player Authenticated...");
                PhotonNetwork.LocalPlayer.NickName = Username;
                GameSparksPanel.SetActive(false);
                PhotonPanel.SetActive(true);
                }
            else
                {
                Debug.Log("Error Authenticating Player...");
                LoginError(response.Errors.JSON);
                SFXManager.Instance.MainSFX.PlayOneShot(ErrorSFX);
                }
        });
        }


    }
