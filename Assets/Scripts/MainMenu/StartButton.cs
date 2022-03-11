using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI startText;

    Color initialColor;
    float fadeDuration = .4f;
    AudioSource startbtn;
    void Awake()
    {
        initialColor = startText.color;
        startbtn = GetComponent<AudioSource>();
    }
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {

        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            startText.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            yield return null;

        }
        StartCoroutine(FadeIn());


    }

    IEnumerator FadeIn()
    {

        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
        Color transparentColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        float elapsedTime = fadeDuration;

        while (elapsedTime > 0f)
        {
            elapsedTime -= Time.deltaTime;
            startText.color = Color.Lerp(targetColor, transparentColor, elapsedTime / fadeDuration);
            yield return null;

        }
        StartCoroutine(FadeOut());
    }
   public void OnClickStart()
    {
        StopAllCoroutines();
        Loader.Load(Loader.Scene.GameScene);
        startbtn.PlayOneShot(startbtn.clip);
    }
}
