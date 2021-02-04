﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{

    public GameObject deathText;
    public GameObject lifeUi;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI levelText;
    public Level level;
    public int currentLevel;
    public int coins;
    public Ball ball;
    public CustomizeManager customizeManager;
    private float currentScore = 0f;
    private int scoreIncreaseRate = 1;
    private float currentMaxScore = 0f;
    private float timer = 0f;
    private bool scoreOverTime = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)
            && !customizeManager.enabled)
        {
            restart();
        }
        timer += Time.unscaledDeltaTime;
        if(timer > 0.1f && scoreOverTime) {
            currentScore += 5f;
            timer = 0;
        }
        if (currentScore < currentMaxScore)
        {
            currentScore = currentScore + scoreIncreaseRate;
        }
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void PlayerDead()
    {
        activateScoreOverTime(false);
        deathText.SetActive(true);
        lifeUi.SetActive(false);
        highScoreText.text = "HighScore: " + currentScore.ToString();
        level.IncrementXp(ball.xp);
        levelText.text = "Level " + level;
        SaveSystem.SavePlayerData(this);

    }

    public void UpdateScore(float score)
    {
        currentMaxScore = this.currentScore + score;
    }

    public void activateScoreOverTime(bool activate)
    {
        scoreOverTime = activate;
    }

    public void restart()
    {
        SceneManager.LoadScene(0);

    }
}
