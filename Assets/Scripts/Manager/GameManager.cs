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

    private void Awake()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    private void Start()
    {
        OnTick += UpdateScore;
        Player.OnCollectCoin += UpdateCoins;
        Player.OnGameOver += GameOver;
    }

    private void Update()
    {
        UpdateTick();
    }

    private void print()
    {
        Debug.Log("pressed");
    }

    private void EndRun()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameOver()
    {
        Player.OnScreenTab += print;
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
