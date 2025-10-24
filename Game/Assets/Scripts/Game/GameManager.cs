using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI gameTimerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("UI")]
    [SerializeField] private GameObject gameResultUI;
    [SerializeField] private GameObject statusBarUI;

    [Header("Button")]
    [SerializeField] private Button MainButton;
    [SerializeField] private Button RestartButton;

    [Header("Timer")]
    [SerializeField] private float gameTime = 120f;
    [SerializeField] private float countdownTime = 3f;

    [Header("GameResult")]
    public RankPrefabManager userRank;
    public GameObject rankPrefab;
    public Transform LeaderBoard;

    private float gametimeCounter = 0f;

    private int score = 0;
    private int gameLife = 3;

    public GameState gameState;

    public enum GameState
    {
        CountDown,
        GameRun,
        Stop,
        GameEnd
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameResultUI.SetActive(false);
        statusBarUI.SetActive(true);
        gameTimerText.text = "";

        UpdateLife(gameLife);
        StartCountdown();
    }

    void Update()
    {
        if (gameState == GameState.GameRun)
        {
            DoCountDown();
        }
    }

    public void LifeMinus()
    {
        gameLife--;
        UpdateLife(gameLife);

        if (gameLife == 0)
        {
            isGameOver();
        }
    }

    public void isGameOver()
    {
        gameState = GameState.GameEnd;
        int reaminingTime = (int)(gameTime - gametimeCounter);
        AddScore(reaminingTime * 300);
        AddScore(gameLife * 500);

        GameResult();
    }

    // Count Down function
    public void StartCountdown()
    {
        gameState = GameState.CountDown;
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        float timer = countdownTime;

        while (timer > 0)
        {
            countDownText.text = timer.ToString("0");
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        countDownText.text = "Game Start!";
        yield return new WaitForSeconds(0.5f);

        countDownText.text = "";
        OnGameStart();
    }


    // 게임 타이머
    void UpdateTimerText(float time)
    {
        int roundUpTime = Mathf.CeilToInt(time);       // 남은 시간을 올림 처리
        int minute, second;

        minute = roundUpTime / 60;                     // 분 단위 계산
        second = roundUpTime % 60;                     // 초 단위 계산

        gameTimerText.text = minute.ToString("00") + " : " + second.ToString("00"); // 00:00 형태로 출력
    }


    void DoCountDown()
    {
        gametimeCounter -= Time.deltaTime;               // 매 프레임마다 남은 시간 감소

        if (gametimeCounter < 0)                         // 시간이 다 되었을 때
        {
            gametimeCounter = 0;                         // 남은 시간 0으로 고정
            isGameOver();
        }

        UpdateTimerText(gametimeCounter);                // 남은 시간 표시 업데이트
    }

    public void OnGameStart()
    {
        SupabaseClient.instance.UpdateRemainingPlays();
        gameState = GameState.GameRun;
        gametimeCounter = gameTime;
        UpdateTimerText(gametimeCounter);
    }

    public bool checkGameRun()
    {
        if (gameState == GameState.GameRun)
        {
            return true;
        }

        return false;
    }

    public void OnMainButtonClick()
    {
        SceneManager.LoadScene("login");
    }

    public async void OnRetryButtonClick()
    {
        if (await SupabaseClient.instance.checkRemainingPlays())
        {
            SceneManager.LoadScene("game");
        }
        else
        {
            resultText.text = "모든 기회를 소진하였습니다.";
        }
    }

    public async void setRankBoard()
    {
        List<User> users = await SupabaseClient.instance.GetAllScore();

        User player = users.FirstOrDefault(x => x.id == SupabaseClient.instance.GetUserID());
        int playerRank = users.FindIndex(u => u.id == SupabaseClient.instance.GetUserID()) + 1;

        RankPrefabManager pItem = userRank.GetComponent<RankPrefabManager>();
        pItem.setRankPrefab(playerRank, player.id, player.name, player.department, player.high_score);

        int count = users.Count >= 100 ? 100 : users.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject rankObj = Instantiate(rankPrefab, LeaderBoard);
            rankObj.name = $"rank_{i}";

            RankPrefabManager item = rankObj.GetComponent<RankPrefabManager>();
            item.setRankPrefab(i + 1, users[i].id, users[i].name, users[i].department, users[i].high_score);
        }
    }


    public async void GameResult()
    {
        statusBarUI.SetActive(false);
        gameResultUI.SetActive(true);

        resultText.text = "Your Score : " + score;
        await SupabaseClient.instance.ScoreUpdate(score);

        setRankBoard();
    }

    public void OnMainButtonClik()
    {
        SceneManager.LoadScene("Login");
    }

    public void AddScore(int newScore)
    {
        if ((score + newScore) < 0)
        {
            score = 0;
        }
        else
        {
            score += newScore;
        }
        scoreText.text = "Score : " + score;
    }

    public void UpdateLife(int remainingLives)
    {
        lifeText.text = "Life : " + remainingLives;
    }
}
