using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Params")] 
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private TextMeshProUGUI WinPanel_Txt;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button quitBtn;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        restartBtn.onClick.AddListener(OnClickRestart);
        mainMenuBtn.onClick.AddListener(OnClickReturnToMainMenu);
        quitBtn.onClick.AddListener(OnClickQuitGame);
    }

    public void ShowOrHideWinPanel(string content, bool isShow)
    {
        WinPanel_Txt.SetText(content);
        WinPanel.SetActive(isShow);
    }

    private void OnClickRestart()
    {
        // ADD TRANSITION HERE
        SceneManager.LoadScene(sceneBuildIndex: 1);
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
}
