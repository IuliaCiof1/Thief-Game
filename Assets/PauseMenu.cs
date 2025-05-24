using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    CursorController cursorController;

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
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        cursorController.CursorVisibility(pauseMenuUI.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
