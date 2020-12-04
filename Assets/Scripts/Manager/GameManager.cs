using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Static gamedata




    [SerializeField]
    private PlayerData playerData;
    private bool runActive = false;

    [SerializeField]
    private const float TICK_SEC_INTERVAL = 0.2f;
    private float tickTimer;

    [SerializeField] 
    private TrackManager tManager;
    private int score;
    private int coins;

    private void Start()
    {
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        SubscribeToEvents();
    }

    private void OnDestroy() => UnSubscribeToEvents();

    private void Update()
    {
        UpdateTick();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape)) EndRun();
#endif
    }

    private void SubscribeToEvents()
    {
        MainMenuUIHandler.OnPlay += StartRun;
        Player.OnCollectCoin += UpdateCoins;
        Player.OnGameOver += GameOver;
    }

    private void UnSubscribeToEvents()
    {
        OnTick -= UpdateScore;
        MainMenuUIHandler.OnPlay -= StartRun;
        Player.OnCollectCoin -= UpdateCoins;
        Player.OnGameOver -= GameOver;
        PlayerInput.OnScreenTab -= EndRun;
    }

    private void StartRun()
    {
        OnTick += UpdateScore;
    }

    private void EndRun()
    {
        PlayerInput.OnScreenTab -= EndRun;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void GameOver()
    {
        PlayerInput.OnScreenTab += EndRun;
        UpdateData();
    }

    private void UpdateData()
    {
        playerData.lastScore = score;
        playerData.lastCoins = coins;
        playerData.coins += coins;
        UpdateHigscore();
    }

    private void UpdateHigscore()
    {
        if (playerData.highscore < score)
        {
            playerData.highscore = score;
            StartCoroutine(MySQLController.AddScore(playerData.email, playerData.displayName, score));
        }
    }

    public static Action<int> OnCoinUpdate;
    private void UpdateCoins()
    {
        coins++;
        OnCoinUpdate?.Invoke(coins);
    }

    public static Action<int> OnScoreUpdate;
    private void UpdateScore()
    {
        int os = score;
        //OnScoreUpdate?.Invoke((score += (int)tManager.tileSpeed) - os);
        OnScoreUpdate?.Invoke(score += (int)tManager.tileSpeed);
    }

    public static Action OnTick;
    private void UpdateTick()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer > TICK_SEC_INTERVAL)
        {
            tickTimer -= TICK_SEC_INTERVAL;
            OnTick?.Invoke();
        }
    }
}
