using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControler : MonoBehaviour
{
    public GameObject pusePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject MenuPanel;
    public GameObject CreditsPanel;

    public bool paused;
    public bool winLose;
    // Use this for initialization
    void Start()
    {
        ClosePausePanel();
        CloseWinPanel();
        CloseLosePanel();
        CloseCreditsPanel();
        if (SceneManager.GetActiveScene().name == "Terrain")
        {
            OpenMenuPanel();
            //Debug.Log("TAMOS EN LA 1");
        }
    }

    public void LoadScene(int num)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(num);
        paused = false;
        winLose = false;
    }

    public void OpenPausePanel()
    {
        pusePanel.SetActive(true);
        paused = true;
    }
    public void OpenWinPanel()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0;
        winLose = true;
    }
    public void OpenLosePanel()
    {
        losePanel.SetActive(true);
        winLose = true;
    }

    public void OpenMenuPanel()
    {
        MenuPanel.SetActive(true);
        winLose = true;
    }

    public void OpenCreditsPanel()
    {
        CreditsPanel.SetActive(true);
        winLose = true;
    }

    public void ClosePausePanel()
    {
        pusePanel.SetActive(false);
        paused = false;
    }
    public void CloseWinPanel()
    {
        winPanel.SetActive(false);
        winLose = false;
    }
    public void CloseLosePanel()
    {
        losePanel.SetActive(false);
        winLose = false;
    }
    public void CloseMenuPanel()
    {
        MenuPanel.SetActive(false);
        winLose = false;
    }

    public void CloseCreditsPanel()
    {
        CreditsPanel.SetActive(false);
        winLose = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
