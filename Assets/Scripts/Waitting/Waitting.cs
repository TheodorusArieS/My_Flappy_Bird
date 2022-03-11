using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Waitting : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI jumpText;
    float jigglyDuration = 1f;
    float distance = 30f;
    RectTransform text;
    void Awake()
    {
        text = jumpText.GetComponent<RectTransform>();
        gameObject.SetActive(true);
    }
    void Start()
    {
        StartCoroutine(Up());
        Bird.GetInstance().OnStartedPlaying += On_Started_Playing;
    }

    IEnumerator Up()
    {
        float elapsedTime = 0f;
        while (elapsedTime < jigglyDuration)
        {
            elapsedTime += Time.deltaTime;
            text.position += new Vector3(0, 1, 0) * distance / jigglyDuration * Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Down());
    }
    IEnumerator Down()
    {
        float elapsedTime = 0f;
        while (elapsedTime < jigglyDuration)
        {
            elapsedTime += Time.deltaTime;
            text.position += new Vector3(0, -1, 0) * distance / jigglyDuration * Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Up());
    }
    private void On_Started_Playing(object sender, System.EventArgs e)
    {
        StopAllCoroutines();
        gameObject.SetActive(false);

    }
}
