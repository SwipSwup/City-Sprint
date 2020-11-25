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
        SubscribeToEvents();
        ResetUI();
    }

    private void OnDestroy() => UnSubscribeToEvents();

    public void EnableInGameUI() => inGameUICanvas.enabled = true;
    public void DisableInGameUI() => inGameUICanvas.enabled = false;

    private void UpdateScore(int score) => this.score.text = score.ToString();
    private void UpdateCoins(int coins) => this.coins.text = "Coins: " + coins;

    private void SubscribeToEvents()
    {
        GameManager.OnScoreUpdate += UpdateScore;
        GameManager.OnCoinUpdate += UpdateCoins;
        Player.OnGameOver += DisableInGameUI;
    }

    private void UnSubscribeToEvents()
    {
        GameManager.OnScoreUpdate -= UpdateScore;
        GameManager.OnCoinUpdate -= UpdateCoins;
        Player.OnGameOver -= DisableInGameUI;
    }

    private void ResetUI()
    {
        UpdateScore(0);
        UpdateCoins(0);
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
