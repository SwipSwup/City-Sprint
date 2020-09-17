using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI coins;

    private void Start()
    {
        GameManager.OnScoreUpdate += UpdateScore;
        GameManager.OnCoinUpdate += UpdateCoins;
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
