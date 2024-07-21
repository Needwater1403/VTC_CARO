using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")] 
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private TextMeshProUGUI WinPanel_Txt;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowOrHideWinPanel(int playerWin, bool isShow)
    {
        WinPanel_Txt.SetText($"PLAYER {playerWin} WIN");
        WinPanel.SetActive(isShow);
    }
}
