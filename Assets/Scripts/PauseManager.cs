using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject switchViewBtn;
    public TextMeshProUGUI shotsRemainingText;

    private bool isPaused = false;

    void Awake()
    {
        pauseCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        switchViewBtn.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        shotsRemainingText.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        switchViewBtn.SetActive(false);
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        shotsRemainingText.gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        MissionDemolition.S.shotsTaken = 0;
        MissionDemolition.S.StartLevel();
        pauseCanvas.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
