using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] Toggle tutorialToggle;
    [SerializeField] Button continueButton;

    private void Start()
    {
        if (!SaveSystem.DoesFileExist() || PlayerPrefs.GetInt("endingReached", 0)==1)
            continueButton.interactable = false;
    }

    //public void ContinueGame()
    //{
    //    GetComponent<HomeEntrance>().GoHome();
    //    /SceneManager.LoadScene("Main");
    //}

    public void NewGame()
    {
        SaveSystem.DeleteSaveFile();

        if (tutorialToggle.isOn)
        {
            print("tutorial enabled");
            PlayerPrefs.SetInt("tutorialEnabled", 1);
        }
        else
        {
            print("tutorial disabled");
            PlayerPrefs.SetInt("tutorialEnabled", 0);
        }
        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
