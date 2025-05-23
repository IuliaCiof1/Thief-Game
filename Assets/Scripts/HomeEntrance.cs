using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeEntrance : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    [SerializeField] private Image fadeImage;
    [SerializeField] bool useFadeOutAtBeggining;
    [SerializeField] float beginLoadTime;
    [SerializeField] string nextSceneName;
    [SerializeField] string message;



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
        
      if(useFadeOutAtBeggining)
            StartCoroutine(FadeOutImage());
       
    }

    public void GoHome()
    {
        StartCoroutine(SaveDataScript.Instance.Save());
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

        SceneManager.LoadScene(nextSceneName);
        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Quarters"))
        //    SceneManager.LoadScene("Main");
        //else
        //    SceneManager.LoadScene("Quarters");
    }

  

    IEnumerator FadeOutImage()
    {
        fadeImage.gameObject.SetActive(true);

        float timer = 0f;
        Color color = fadeImage.color;

        color.a = 1;
        fadeImage.color = color;

        yield return new WaitForSeconds(beginLoadTime);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Clamp01(1 - (timer / fadeDuration));
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }

   public string GetMessage()
    {
        return message;
    }
}
