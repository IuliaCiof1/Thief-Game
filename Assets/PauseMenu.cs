using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject sureQuitGameUI;
    CursorController cursorController;
    bool iniCursorVisibility;

    private void Start()
    {
        cursorController = FindFirstObjectByType<CursorController>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchPauseMenu();
        }
    }

    public void SwitchPauseMenu()
    {
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);

        if (pauseMenuUI.activeSelf)
        {
            cursorController.CursorVisibility(pauseMenuUI.activeSelf);
            Time.timeScale = 0;
        }
        else
        {
            sureQuitGameUI.SetActive(false);
            cursorController.SetToPreveousCursorState();
            Time.timeScale = 1;
        }

        
    }

    

    public void QuitGame()
    {
        Application.Quit();
    }

}
