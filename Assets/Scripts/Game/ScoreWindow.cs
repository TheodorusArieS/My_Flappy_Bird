using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ScoreWindow : MonoBehaviour
{
    TextMeshProUGUI highScoreText;
    TextMeshProUGUI scoreText;
    void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        highScoreText = transform.Find("HighScoreText").GetComponent<TextMeshProUGUI>();

    }

    void Start()
    {
        if (PlayerPrefs.GetInt("highscore") == 0)
        {
            highScoreText.text = "Highscore : " + "0";
        }
        else
        {
            highScoreText.text = "Highscore : " + PlayerPrefs.GetInt("highscore").ToString();
        }
    }

    void Update()
    {
        scoreText.text = Level.GetInstance().GetScore().ToString();
    }

}
