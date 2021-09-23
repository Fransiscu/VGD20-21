using Prime31.TransitionKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private bool gamePaused = false;
    public GameObject menuCanvas;

    private void Awake()
    {
        menuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        gamePaused = true;
        menuCanvas.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        gamePaused = false;
        menuCanvas.SetActive(false);
    }

    public void OnResumeButtonPress()
    {
        ResumeGame();
    }

    public void OnMainMenuButtonPress()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void OnQuitButtonPress()
    {
        FadeTransition fadeToLevel = new FadeTransition()
        {
            fadedDelay = .5f,
            duration = 1.5f,
            fadeToColor = Color.black
        };
        TransitionKit.instance.transitionWithDelegate(fadeToLevel);
        Application.Quit();
    }
    
    public void OnLastSaveButtonPress()
    {
        int currentLevel = GameController.GetCurrentGameLevel();
        StartCoroutine("StartLevel", currentLevel);
    }

    public IEnumerator StartLevel(int sceneNumber)
    {
        Debug.LogWarning("startin " + sceneNumber + " level");
        SceneManager.LoadScene(sceneNumber + 1);
        yield return new WaitForSeconds(1f);
    }
}
