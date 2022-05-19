using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Camera _mainCamera;
    
    [Header("Game Settings")]
    public int score;
    public int highScore;
    public float timeRemaining = 60;
    public bool isTimerRunning;

    public static GameManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            RaycastHit hit;
            
            Debug.DrawRay(_mainCamera.transform.position, _mainCamera.ScreenPointToRay(Input.mousePosition).GetPoint(20), Color.red, 3);

            if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 50))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    var enemy = hit.collider.gameObject.GetComponent<Enemy>();
                    EnemyManager.Instance.ClickedOnEnemy(enemy);
                }
            }
        }

        if (!isTimerRunning) return;
        
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
            isTimerRunning = false;
            Debug.Log("Time is up!");
            highScore = Mathf.Max(score, highScore);
        }
        
        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateTimer(timeRemaining);
    }

}
