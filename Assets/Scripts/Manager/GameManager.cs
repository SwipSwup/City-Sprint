using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private float TICK_SEC_INTERVAL;
    private float tickTimer;

    [SerializeField] private TrackManager tManager;
    private int score;

    private void Start()
    {
        OnTick += UpdateScore;
    }

    private void Update()
    {
        UpdateTick();
    }

    public static Action<int> OnScoreUpdate;
    private void UpdateScore()
    {
        int os = score;
        OnScoreUpdate?.Invoke((score += (int)tManager.tileSpeed) - os);
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
