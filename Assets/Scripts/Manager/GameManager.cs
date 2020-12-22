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
    private const float TICK_SEC_INTERVAL = 0.2f;
    private float tickTimer;

    private TrackManager tManager;
    private int score;
    private int coins;

    private void Start()
    {
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        tManager = GetComponent<TrackManager>();
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
    private void CreateNewPlayer(string email, string username)
    {
        playerData.email = email;
        playerData.displayName = username;

        playerData.place = 0;
        playerData.lastScore = 0;
        playerData.highscore = 0;
        playerData.lastCoins = 0;
        playerData.coins = 0;
    } 

    private void SubscribeToEvents()
    {
        MainMenuUIHandler.OnPlay += StartRun;
        Player.OnCollectCoin += UpdateCoins;
        Player.OnGameOver += GameOver;
        ConnectPopUpHandler.OnUserRegister += CreateNewPlayer;
    }

    private void UnSubscribeToEvents()
    {
        OnTick -= UpdateScore;
        MainMenuUIHandler.OnPlay -= StartRun;
        Player.OnCollectCoin -= UpdateCoins;
        Player.OnGameOver -= GameOver;
        PlayerInput.OnScreenTab -= EndRun;
        ConnectPopUpHandler.OnUserRegister -= CreateNewPlayer;
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
