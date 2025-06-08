using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    [SerializeField] TMP_Text textUI;
    [SerializeField] GameObject UI;
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration;

    private void Awake()
    {
        EndingManager.endingEvent += HandleEnding;
    }

    private void OnDisable()
    {
        EndingManager.endingEvent -= HandleEnding;
    }

    void HandleEnding(string endText)
    {
        PlayerPrefs.SetInt("endingReached", 1);
        FindFirstObjectByType<CursorController>().CursorVisibility(true);
        
        UI.SetActive(true);
        textUI.text = endText;


        fadeImage.gameObject.SetActive(true);
        textUI.gameObject.SetActive(true);

        StartCoroutine(FadeInImage());
    }

    IEnumerator FadeInImage()
    {
        fadeImage.gameObject.SetActive(true);
        float timer = 0f;
        Color imageColor = fadeImage.color;
        Color textColor = textUI.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            imageColor.a = Mathf.Clamp01(timer / fadeDuration);
            textColor.a = imageColor.a;

            fadeImage.color = imageColor;
            textUI.color = textColor;

            yield return null;
        }
        Time.timeScale = 0f;
    }
}
