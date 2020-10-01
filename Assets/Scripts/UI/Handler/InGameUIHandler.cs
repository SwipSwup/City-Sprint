using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private Canvas inGameUICanvas;

    private void Start()
    {
        GameManager.OnScoreUpdate += UpdateScore;
        GameManager.OnCoinUpdate += UpdateCoins;
        Player.OnGameOver += DisableUI;

        ResetUI();
    }

    private void ResetUI()
    {
        UpdateScore(0);
        UpdateCoins(0);
    }

    private void DisableUI()
    {
        Debug.Log("bruh");
        inGameUICanvas.enabled = false;
    }

    private void EnableUI()
    {
        inGameUICanvas.enabled = true;
    }

    private void UpdateScore(int score)
    {
        this.score.text = "Score: " + score;
    }

    private void UpdateCoins(int coins)
    {
        this.coins.text = "Coins: " + coins;
    }

    private IEnumerator CountUp(int score)
    {
        for (int i = 0; i < score; i++)
        {
            string scoreText = "Score: " + (int.Parse(this.score.text) + 1); 

            this.score.text = scoreText;
            yield return new WaitForSeconds(.3f);
        }
    }
}
