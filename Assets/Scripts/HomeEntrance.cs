using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeEntrance : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    [SerializeField] private Image fadeImage;
    [SerializeField] bool toHome;

    private void OnEnable()
    {
        TimeManager.OnDayEnded += GoHome;
    }

    private void OnDisable()
    {
        TimeManager.OnDayEnded -= GoHome;
    }

    private void Start()
    {
        
      
            StartCoroutine(FadeOutImage());
       
    }

    public void GoHome()
    {
       
            StartCoroutine(FadeInImage());
       
    }

    IEnumerator FadeInImage()
    {
        fadeImage.gameObject.SetActive(true);
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Quarters"))
            SceneManager.LoadScene("Main");
        else
            SceneManager.LoadScene("Quarters");
    }



    IEnumerator FadeOutImage()
    {


        print("fade out");
        float timer = 0f;
        Color color = fadeImage.color;

        color.a = 1;
        fadeImage.color = color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Clamp01(1 - (timer / fadeDuration));
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }
}
