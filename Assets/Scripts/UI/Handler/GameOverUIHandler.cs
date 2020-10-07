using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUIHandler : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI coins;

    [SerializeField] private Canvas gameOverCanvas;

    private void Start()
    {
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnSubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        Player.OnGameOver += OpenGameOverMenu;
    }

    private void UnSubscribeToEvents()
    {
        Player.OnGameOver -= OpenGameOverMenu;
    }

    private void CloseGameOverMenu()
    {
        gameOverCanvas.enabled = false;
    }

    private void OpenGameOverMenu()
    {
        gameOverCanvas.enabled = true;
        ChangeScore(data.lastScore);
        ChangeCoins(data.lastCoins);
    }



    private void ChangeScore(int score)
    {
        this.score.text = "Score: " + score;
    }

    private void ChangeCoins(int coins)
    {
        this.coins.text = "Coins: " + data.lastCoins;
    }
}
