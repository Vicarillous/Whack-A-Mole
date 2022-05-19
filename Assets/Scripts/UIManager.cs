using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    
    private GameObject _inGameUI;
    private GameObject _mainMenuUI;

    private void Awake()
    {
        _inGameUI = GameObject.Find("InGame");
        _inGameUI.SetActive(false);
        
        _mainMenuUI = GameObject.Find("MainMenu");
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }
    
    public void UpdateTimer(float time)
    {
        _timerText.text = time.ToString("F0");
    }

    public void StartNewGame()
    {
        GameManager.Instance.isTimerRunning = true;

        _inGameUI.SetActive(true);
        _mainMenuUI.SetActive(false);
    }
}
