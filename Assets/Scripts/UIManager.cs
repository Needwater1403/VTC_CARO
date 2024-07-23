using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Params")] 
    [SerializeField] private GameObject GameConfigPanel;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private TextMeshProUGUI WinPanel_Txt;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button startBtn;
    [SerializeField] private TMP_Dropdown boardSizeDropdown;
    [SerializeField] private TMP_Dropdown gameModeDropdown;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        restartBtn.onClick.AddListener(OnClickRestart);
        mainMenuBtn.onClick.AddListener(OnClickReturnToMainMenu);
        startBtn.onClick.AddListener(OnClickStartGame);
        quitBtn.onClick.AddListener(OnClickQuitGame);
        boardSizeDropdown.onValueChanged.AddListener(ChangeBoardSize);
        gameModeDropdown.onValueChanged.AddListener(ChangeGameMode);
    }

    public void ShowOrHideWinPanel(string content, bool isShow)
    {
        WinPanel_Txt.SetText(content);
        WinPanel.SetActive(isShow);
    }

    private void OnClickRestart()
    {
        // ADD TRANSITION HERE
        GameScript.Instance.ResetGame();
        WinPanel.SetActive(false);
        GameConfigPanel.SetActive(true);
    }
    
    private void OnClickReturnToMainMenu()
    {
        // ADD TRANSITION HERE
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
    private void OnClickQuitGame()
    {
        Application.Quit();
    }
    private void ChangeBoardSize(int a)
    {
        var temp = boardSizeDropdown.options[a].text;
        switch(temp)
        {
            case "3x3":
                //Debug.Log("3X3");
                GameScript.Instance.size = 3;
                break;
            case "5x5":
                //Debug.Log("5X5");
                GameScript.Instance.size = 5;
                break;
        }
    }
    private void ChangeGameMode(int a)
    {
        var temp = gameModeDropdown.options[a].text;
        switch(temp)
        {
            case "EASY":
                //Debug.Log("EASY");
                GameScript.Instance.Mode = GameScript.Difficulty.EASY;
                break;
            case "HARD":
                //Debug.Log("HARD");
                GameScript.Instance.Mode = GameScript.Difficulty.HARD;
                break;
        }
    }

    private void OnClickStartGame()
    {
        GameConfigPanel.SetActive(false);
        GameScript.Instance.StartGame();
    }
    
}
