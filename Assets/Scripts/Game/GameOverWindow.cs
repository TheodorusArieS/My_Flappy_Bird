using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOverWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI highscoreText;
    AudioSource retryBtn;
    AudioSource mainmenuBtn;
    void Awake()
    {
        retryBtn = transform.Find("Retry").GetComponent<AudioSource>();
        mainmenuBtn = transform.Find("MainMenu").GetComponent<AudioSource>();
    }
    void Start()
    {
        Bird.GetInstance().OnDied += Bird_On_Died;
        Hide();
    }

    void Bird_On_Died(object sender, System.EventArgs e)
    {
        Show();
        scoreText.text = "Score : " + Level.GetInstance().GetScore().ToString();
    }

    public void OnClickRetry()
    {
        Loader.Load(Loader.Scene.GameScene);
        retryBtn.PlayOneShot(retryBtn.clip);
    }

    public void OnClickMainMenu()
    {
        Loader.Load(Loader.Scene.MainMenu);
        mainmenuBtn.PlayOneShot(mainmenuBtn.clip);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
    void Show()
    {
        gameObject.SetActive(true);
    }
}
