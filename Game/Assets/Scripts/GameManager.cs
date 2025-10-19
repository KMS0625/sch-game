using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameover = false;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI resultText;
    public Button MainButton;
    public GameObject gameoverUI;

    private int score = 0;

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
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool checkGameover()
    {
        return isGameover;
    }

    public void GameResult()
    {
        isGameover = true;
        gameoverUI.SetActive(true);

        resultText.text = "Your Score : " + score;
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
