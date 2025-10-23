using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI countText;
    public Button MainButton;
    public GameObject gameoverUI;
    public GameObject gamestartUI;
    public TextMeshProUGUI timer_text;
    public GameObject playingPanel;

    public float countdownTime = 3f;
    public float countDownCounter = 0;
    GameState gameState = GameState.GameStart;
    private int score = 0;
    private float timeRemaining;
    public float gameTime = 10f;
    public int gameLife = 3;

    public void LifeMinus()
    {
        gameLife--;
        UpdateLife(gameLife);

        if (gameLife == 0)
        {
            gameState = GameState.GameEnd;
        }
    }

    public enum GameState
    {
        CountDown,
        GameStart,
        Paused,
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
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다.");
            Destroy(gameObject);
        }

        gameoverUI.SetActive(false);
        StartCountdown();
        timeRemaining = gameTime;
        UpdateTimerText(timeRemaining);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.GameStart)
        {
            DoCountDown();
        }

        if (gameState == GameState.GameEnd)
        {
            GameResult();
        }
    }

    public void isGameOver()
    {
        gameState = GameState.GameEnd;
    }

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
            countText.text = timer.ToString("0");
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        countText.text = "Game Start!";
        gamestartUI.SetActive(false);
        playingPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        OnGameStart();
    }

    // 시간 텍스트를 분:초(00:00) 형식으로 변환하여 표시
    void UpdateTimerText(float time)
    {
        int roundUpTime = Mathf.CeilToInt(time);       // 남은 시간을 올림 처리
        int minute, second;

        minute = roundUpTime / 60;                     // 분 단위 계산
        second = roundUpTime % 60;                     // 초 단위 계산

        timer_text.text = minute.ToString("00") + " : " + second.ToString("00"); // 00:00 형태로 출력
    }

    // 카운트다운 실행
    void DoCountDown()
    {
        timeRemaining -= Time.deltaTime;               // 매 프레임마다 남은 시간 감소

        if (timeRemaining < 0)                         // 시간이 다 되었을 때
        {
            timeRemaining = 0;                         // 남은 시간 0으로 고정
            timer_text.text = "start";                 // 버튼 텍스트 초기화
            isGameOver();
        }

        UpdateTimerText(timeRemaining);                // 남은 시간 표시 업데이트
        Debug.Log("Time remaining = " + timeRemaining);
    }

    public void OnGameStart()
    {
        gameState = GameState.GameStart;
    }

    public bool checkGameStart()
    {
        if (gameState == GameState.GameStart)
        {
            return true;
        }

        return false;
    }

    public async void GameResult()
    {
        gameState = GameState.GameEnd;
        playingPanel.SetActive(false);
        gameoverUI.SetActive(true);

        resultText.text = "Your Score : " + score;

        await SupabaseClient.instance.ScoreUpdate(score);
    }

    public void OnMainButtonClik()
    {
        SceneManager.LoadScene("Login");
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int newScore)
    {
        score += newScore;
        scoreText.text = "Score : " + score;
    }

    public void UpdateLife(int remainingLives)
    {
        lifeText.text = "Life : " + remainingLives;
    }
}
