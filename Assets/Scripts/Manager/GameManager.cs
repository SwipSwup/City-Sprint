using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private float TICK_SEC_INTERVAL;
    private float tickTimer;

    [SerializeField] private TrackManager tManager;
    private int score;
    private int coins;

    private void Start()
    {
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);

        Debug.Log("loaded");

        OnTick += UpdateScore;
        Player.OnCollectCoin += UpdateCoins;
        Player.OnGameOver += GameOver;
    }

    private void Update()
    {
        UpdateTick();
        if (Input.GetKey(KeyCode.Escape)) EndRun();
    }

    private void EndRun()
    {
        Player.OnScreenTab -= EndRun;
        SceneManager.UnloadSceneAsync("UI");
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
    }

    private void GameOver()
    {
        Player.OnScreenTab += EndRun;
        playerData.lastScore = score;
        if (playerData.highscore < score) playerData.highscore = score;
        playerData.coins += coins;
        playerData.lastCoins = coins;
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
