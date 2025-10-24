using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Login : MonoBehaviour
{
    public enum LoginState
    {
        Success,
        UserIdNotFound,
        NameMismatch,
        DpartmentMisMatch,
        AccountLocked
    };

    [Header("Login Input Field")]
    public TMP_InputField inputLoginName;
    public TMP_InputField inputLoginID;
    public TMP_InputField inputLoginDepartment;

    [Header("Signup Input Field")]
    public TMP_InputField inputSignupName;
    public TMP_InputField inputSingupID;
    public TMP_InputField inputSighupDpartment;

    [Header("LoginPanel Button")]
    public Button loginButton;
    public Button CreateAccountButton;

    [Header("Close Button")]
    public Button CloseLoginButton;
    public Button CloseSignupButton;
    public Button CloseRankingButton;

    [Header("UI")]
    public GameObject loginPanel;
    public GameObject signupPanel;
    public GameObject mainPanel;
    public GameObject rankPanel;
    public GameObject rulePanel;

    [Header("Text")]
    public TextMeshProUGUI loginResult;
    public TextMeshProUGUI signupResult;

    [Header("Ranking")]
    public GameObject rankPrefab;
    public Transform LeaderBoard;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720, true);
        SupabaseClient.instance.SetUserID(null);
    }

    public void OnGameStartButtonClick()
    {
        mainPanel.SetActive(false);

        LoginPanelReset();
        loginPanel.SetActive(true);
    }

    public async void OnLoginButtonClickEvent()
    {
        string name = inputLoginName.text;
        string id = inputLoginID.text;
        string department = inputLoginDepartment.text;

        LoginState response = await SupabaseClient.instance.Login(id, name, department);

        if (response == LoginState.UserIdNotFound)
        {
            loginResult.text = "존재하지 않는 아이디입니다. 회원가입을 진행해주세요!";
        }
        else if (response == LoginState.DpartmentMisMatch)
        {
            loginResult.text = "학과가 일치하지 않습니다!";
        }
        else if (response == LoginState.NameMismatch)
        {
            loginResult.text = "이름이 일치하지 않습니다!";
        }
        else if (response == LoginState.AccountLocked)
        {
            loginResult.text = "게임 기회를 모두 소진하였습니다";
        }

        if (response == LoginState.Success)
        {
            Debug.Log("로그인 성공!");
            SceneManager.LoadScene("game");
        }
    }

    public void OnRankingButtonClick()
    {
        rankPanel.SetActive(true);
        Ranking();
    }

    public void OnCreateAccoumtButtonClick()
    {
        signupPanel.SetActive(true);
        LoginPanelReset();
    }

    public void OnCloseLoginButtonClick()
    {
        LoginPanelReset();
        loginPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void OnCloseSignupButtonClick()
    {
        SignupPanelReset();
        signupPanel.SetActive(false);
    }

    public void OnCloseRankingButtonClick()
    {
        RankPanelReset();
        rankPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void RankPanelReset()
    {
        foreach (Transform child in LeaderBoard)
        {
            Destroy(child.gameObject);
        }
    }

    public void LoginPanelReset()
    {
        inputLoginName.text = "";
        inputLoginID.text = "";
        inputLoginDepartment.text = "";
        loginResult.text = "";
    }

    public void SignupPanelReset()
    {
        inputSignupName.text = "";
        inputSingupID.text = "";
        inputSighupDpartment.text = "";
        signupResult.text = "";
    }

    public async void OnSignupClick()
    {
        string name = inputSignupName.text;
        string id = inputSingupID.text;
        string department = inputSighupDpartment.text;

        if (name == "" || id == "" || department == "")
        {
            signupResult.text = "모두 입력해주세요!";
            return;
        }

        bool access = await SupabaseClient.instance.Signup(id, name, department);
        if (access)
        {
            signupPanel.SetActive(false);
            inputSingupID.text = "";
            inputSignupName.text = "";
            inputSighupDpartment.text = "";
        }
        else
        {
            signupResult.text = "이미 존재하는 아이디입니다!";
        }
    }

    public async void Ranking()
    {
        List<User> users = await SupabaseClient.instance.GetAllScore();

        if (users == null || users.Count == 0)
        {
            Debug.Log("랭킹 데이터가 없습니다.");
            return;
        }


        int count = users.Count >= 100 ? 100 : users.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject rankObj = Instantiate(rankPrefab, LeaderBoard);
            rankObj.name = $"rank_{i}";

            RankPrefabManager item = rankObj.GetComponent<RankPrefabManager>();
            item.setRankPrefab(i + 1, users[i].id, users[i].name, users[i].department, users[i].high_score);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void OnRuleCloseButtonClick()
    {
        rulePanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void OnRuleButtonClick()
    {
        mainPanel.SetActive(false);
        rulePanel.SetActive(true);
    }
}
