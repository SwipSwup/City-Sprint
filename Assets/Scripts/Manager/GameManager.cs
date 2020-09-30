using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        OnTick += UpdateScore;
        Player.OnCollectCoin += UpdateCoins;
        Player.OnGameOver += GameOver;
    }

    private void Update()
    {
        UpdateTick();
    }

    private void EndRun()
    {



    }

    private void GameOver()
    {
        EndRun();
        if(playerData.highscore < score) playerData.highscore = score;
        playerData.coins += coins;
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
