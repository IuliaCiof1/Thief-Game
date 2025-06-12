using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject sureQuitGameUI;
    CursorController cursorController;
    bool iniCursorVisibility;

    private void Start()
    {
        cursorController = FindFirstObjectByType<CursorController>();
        Time.timeScale = 1;
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

    

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
