using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class login : MonoBehaviour
{
    public TMP_InputField inputUserName;
    public TMP_InputField inputPassword;
    public GameObject loginButton;
    public GameObject CreateAccountButton;
    public GameObject CreateAccountButtonPanel;

    public static string userNameData;
    public static string userPWDate;

    string LoginURL = "http://127.0.0.1/Login.php";

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720, true);
    }

    // Update is called once per frame
    void Update()
    {
        userNameData = inputUserName.text;
        userPWDate = inputPassword.text;
    }

    public void OnLoginButtonClickEvent()
    {
        string userName = inputUserName.text;
        string password = inputPassword.text;

        StartCoroutine(LoginToDB(userName, password));
        Debug.Log("UserName: " + userName);
        Debug.Log("Password: " + password);
    }

    public void OnCreateAccoumtButtonEvent()
    {
        CreateAccountButtonPanel.SetActive(true);
    }

    IEnumerator LoginToDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("namePost", username);
        form.AddField("passPost", password);

        WWW www = new WWW(LoginURL, form);

        yield return www;
        Debug.Log(www.text);

        var text = www.text.Trim();
        if(text.Contains("login success"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
