using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    [SerializeField] private GameObject[] enemies;

    private GameObject _currentEnemy;
    private Animator _currentEnemyAnimator;
    private Enemy _currentEnemyScript;
    private static readonly int IsUp = Animator.StringToHash("isUp");
    
    [SerializeField] private float interval = 1f;
    [SerializeField] private float timeUp = 2f;
    
    private bool _playerDidAPoint;
    private bool _nextEnemyCreated;
    private bool _timeEnded;
    private bool _intervalEnded;

    private float _time;
    private float _timeInterval;
    private float _waitTime;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GameObject.Find("SmashAudio").GetComponent<AudioSource>();
    }

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
    }

    private void Update()
    {
        if (GameManager.Instance.isTimerRunning)
        {
            if (_playerDidAPoint || _timeEnded)
            {

                if (_currentEnemyScript.isUp)
                {
                    _waitTime += Time.deltaTime;
                    if (_waitTime >= 0.5f)
                    {
                        _currentEnemyScript.isUp = false;
                        _waitTime = 0;
                    }
                }
                else
                {
                    DownEnemy();
                    
                    if (!_intervalEnded)
                    {
                        _timeInterval += Time.deltaTime;
                        if (_timeInterval >= interval)
                        {
                            _intervalEnded = true;
                            _timeInterval = 0;
                        }
                    }

                    if (_intervalEnded)
                    {
                        NextEnemy();
                    }
                }
                
                
            }
            else if (!_nextEnemyCreated)
            {
                NextEnemy();
            }
            
            if (!_timeEnded)
            {
                _time += Time.deltaTime;
                if (_time >= timeUp)
                {
                    _time = 0;
                    _timeEnded = true;
                }
            }
        }
    }

    private void NextEnemy()
    {
        _nextEnemyCreated = true;
        
        _playerDidAPoint = false;
        _timeEnded = false;
        _time = 0;
        _timeInterval = 0;
        _intervalEnded = false;
        
        var randomValue = Random.Range(0, enemies.Length);
            
        _currentEnemy = enemies[(randomValue)];
        _currentEnemyAnimator = _currentEnemy.GetComponent<Animator>();
        _currentEnemyScript = _currentEnemy.GetComponent<Enemy>();

        UpEnemy();
    }

    /*private IEnumerator Logic()
    {
        do
        {

            var randomValue = Random.Range(0, enemies.Length);
            
            _currentEnemy = enemies[(randomValue)];
            _currentEnemyAnimator = _currentEnemy.GetComponent<Animator>();
            _currentEnemyScript = _currentEnemy.GetComponent<Enemy>();

            UpEnemy();
            
            Debug.Log("Enemy" + randomValue + "Up");
            
            //yield return new WaitForSeconds(2);
            
            DownEnemy();
            
        } while (GameManager.Instance.IsTimerRunning());

        yield return null;
    }*/

    public void ClickedOnEnemy(Enemy enemy)
    {
        if (enemy.isUp)
        {
            GameManager.Instance.score++;
            _playerDidAPoint = true;
            timeUp -= 0.1f;
            timeUp = Mathf.Clamp(timeUp, 0.5f, float.MaxValue);
            interval -= 0.1f;
            interval = Mathf.Clamp(interval, 0.5f, float.MaxValue);
            
            _audioSource.Play();

            DownEnemy();
        }
    }

    private void UpEnemy()
    {
        _currentEnemyScript.isUp = true;

        _currentEnemyAnimator.SetBool(IsUp, true);
        
        _currentEnemyAnimator.Play("Up");
    }

    private void DownEnemy()
    {
        _currentEnemyAnimator.SetBool(IsUp, false);
        
        _currentEnemyAnimator.Play("Down");

        Debug.Log("EnemyDown");
        
        _currentEnemyScript.isUp = false;
    }
}
