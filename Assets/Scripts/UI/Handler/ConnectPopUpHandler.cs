using System;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class ConnectPopUpHandler : MonoBehaviour
{
    [SerializeField] private Canvas PopUpCanvas;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject loginUI;
    [SerializeField] private GameObject registerUI;

    [SerializeField] private TMP_InputField loginEmail;
    [SerializeField] private TMP_InputField loginPassword;
    [SerializeField] private TMP_InputField registerEmail;
    [SerializeField] private TMP_InputField registerUsername;
    [SerializeField] private TMP_InputField registerPassword;
    [SerializeField] private TMP_InputField registerPasswordConfirm;

    [SerializeField] private TextMeshProUGUI loginError;
    [SerializeField] private TextMeshProUGUI registerError;

    private bool IsEmailValid(string email) =>
        Regex.IsMatch(
        email,
        @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
        RegexOptions.IgnoreCase
        );

    public static Action<string, string> OnUserLogin;
    public void Login()
    {
        if (!IsEmailValid(loginEmail.text))
            SetLoginErrorMsg("Email address is invalid");
        else if (false /* MySQLController.login() */)
            SetLoginErrorMsg("Email or password incorrect");
        else
        {
            //todo: OnUserLogin        
            closePopUp();
        }
    }

    public static Action<string, string> OnUserRegister;
    public void Register()
    {
        if (!IsEmailValid(registerEmail.text))
            SetRegisterErrorMsg("Email address is invalid.");
        else if (registerPassword.text.Length < 5)
            SetRegisterErrorMsg("The password hast to be at least 5 characters long");
        else if (registerPassword.text != registerPasswordConfirm.text)
            SetRegisterErrorMsg("The passwords don't match.");
        else if (false /* await MySQLController.UserExist() */)
            SetRegisterErrorMsg("Their is already a user with this email addres.");
        else if (false /* await MySQLController.UsernameExist() */)
            SetRegisterErrorMsg("This username already exists.");
        else if (false /* await MySQLController.register() */)
            SetLoginErrorMsg("There was a problem creating a new account. Try again later.");
        else
        {
            OnUserRegister?.Invoke(registerEmail.text, registerUsername.text);
            closePopUp();
        }
    }

    public void SetLoginErrorMsg(string errMsg) => loginError.text = errMsg;
    public void SetRegisterErrorMsg(string errMsg) => registerError.text = errMsg;

    public void closePopUp() => PopUpCanvas.enabled = false;
    public void openPopUp() => PopUpCanvas.enabled = true;

    public void disableMainUI() => mainUI.SetActive(false);
    public void disableLoginUI() => loginUI.SetActive(false);
    public void disableRegisterUI() => registerUI.SetActive(false);

    public void enableMainUI() => mainUI.SetActive(true);
    public void enableLoginUI() => loginUI.SetActive(true);
    public void enableRegisterUI() => registerUI.SetActive(true);

}
