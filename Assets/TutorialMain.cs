using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMain : MonoBehaviour
{

    [SerializeField] GameObject[] tutorialPanels;
    [SerializeField] Transform warpPointsParent;
    [SerializeField] Transform[] objectsToEnable;
    [SerializeField] Transform[] objectsToDisable;

    private int currentStage = 0;
    public static bool uiActive { get; private set; }
    private ThirdPersonController player;


    public static Action OnPickpocket;
    public static Action OnBeginPickpocket;
    public static Action OnItemCollected;
    public static Action OnClosePickpocket;
    public static Action OnFinishTutorial;

    public static Action OnCloseHome;
    public static Action OnBeginFurniture;
    public static Action OnFurniture;


    private void Awake()
    {
    
        if (PlayerPrefs.GetInt("tutorialEnabled") != 1)
        {
            EnableTutorialObjects(false);
            return;
        }

        player = FindObjectOfType<ThirdPersonController>();
        EnableTutorialObjects(true);
        WarpToStage(0);
        ShowPanel(currentStage);
        uiActive = true;
        
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main"))
        {
            OnPickpocket += HandlePickpocket;
            OnItemCollected += HandleItemCollected;
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Quarters"))
        {
            OnCloseHome += HandleOnCloseHome;
        }

        Time.timeScale = 0f;
    }

    private void UnsubscribeFromEvents()
    {
        OnPickpocket -= HandlePickpocket;
        OnItemCollected -= HandleItemCollected;
        OnBeginPickpocket -= HandleBeginPickpocket;
        OnClosePickpocket -= HandleClosePickpocket;
        OnFinishTutorial -= FinishTutorial;
        OnFurniture -= HandleFurniture;
        OnBeginFurniture -= HandleBeginFurniture;
        OnCloseHome -= HandleOnCloseHome;
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void Update()
    {
        if (uiActive && Input.GetKeyDown(KeyCode.Q))
        {
            print("pressed Q to close");
           
            HidePanel(currentStage);
           
            Time.timeScale = 1f;
            uiActive = false;
            OnBeginPickpocket?.Invoke();
            OnFinishTutorial?.Invoke();
            OnCloseHome?.Invoke();
            OnBeginFurniture?.Invoke();
            
        }
    }

    private void HandlePickpocket()
    {
        AdvanceToStage(1);
        OnBeginPickpocket += HandleBeginPickpocket;

    }

    private void HandleBeginPickpocket()
    {
        print("handlebeginpickpocket");
        AdvanceToStage(2);
        OnBeginPickpocket -= HandleBeginPickpocket;
    }

    private void HandleItemCollected()
    {
        AdvanceToStage(3);
        OnClosePickpocket += HandleClosePickpocket;
    }


    private void HandleClosePickpocket()
    {
        //warp player near home
        WarpToStage(1);
        AdvanceToStage(4);
        Time.timeScale = 1f;
       
        uiActive = false;
        OnClosePickpocket -= HandleClosePickpocket;
        //OnFinishTutorial += FinishTutorial;
    }

    void FinishTutorial()
    {
        //PlayerPrefs.SetInt("tutorialEnabled", 0);
        
        EnableTutorialObjects(false);
        UnsubscribeFromEvents();
    }

    private void AdvanceToStage(int nextStage)
    {
        print("advance to stage " + nextStage);
        HidePanel(currentStage);
        currentStage = nextStage;
        ShowPanel(currentStage);
        Time.timeScale = 0f;
        uiActive = true;
    }

    private void ShowPanel(int index)
    {
        print("show panel " + index + " " + tutorialPanels.Length);
        if (index < tutorialPanels.Length)
        {
            print(tutorialPanels[index].name);
            tutorialPanels[index].SetActive(true);
        }
    }

    private void HidePanel(int index)
    {
        if (index < tutorialPanels.Length)
            tutorialPanels[index].SetActive(false);
    }

    private void WarpToStage(int index)
    {
        player.gameObject.SetActive(false);
        player.transform.position = warpPointsParent.GetChild(index).position;
        player.gameObject.SetActive(true);
    }

    private void EnableTutorialObjects(bool value)
    {
        foreach (Transform obj in objectsToEnable)
            obj.gameObject.SetActive(value);

        foreach (Transform obj in objectsToDisable)
        {
           
            obj.gameObject.SetActive(!value);
        }
    }

    private void HandleOnCloseHome()
    {
        print("handle onclosehome");
        OnCloseHome -= HandleOnCloseHome;
        AdvanceToStage(1);
        OnBeginFurniture += HandleBeginFurniture;
    }

    private void HandleBeginFurniture()
    {
        PlayerStats.Instance.AddMoiney(5);
        OnBeginFurniture -= HandleBeginFurniture;
        AdvanceToStage(2);
        OnFurniture += HandleFurniture;
    }

    private void HandleFurniture()
    {
        
        AdvanceToStage(3);
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("tutorialEnabled", 0);
        OnFinishTutorial += FinishTutorial;
    }
}
