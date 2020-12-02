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

    private void Start() => SubscribeToEvents();
    private void OnDestroy() => UnSubscribeToEvents();

    private void SubscribeToEvents()
    {
        Player.OnGameOver += OpenGameOverMenu;
    }

    private void UnSubscribeToEvents()
    {
        Player.OnGameOver -= OpenGameOverMenu;
    }

    private void CloseGameOverMenu() => gameOverCanvas.enabled = false;

    private void OpenGameOverMenu()
    {
        gameOverCanvas.enabled = true;
        StartCoroutine(ShowInfo());
        ChangeCoins(data.lastCoins);
    }

    private IEnumerator ShowInfo()
    {
        ChangeScore(0);

        int lastScore = data.lastScore;
        int tmp = 0;
        Debug.Log(lastScore);
        yield return new WaitForSeconds(.5f);
        while(tmp < data.lastScore)
        {
          //  ChangeScore(tmp %= lastScore);
            lastScore /= 5;
            Debug.Log(lastScore);

            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(1f);
        showCoins();
    }

    private void showCoins()
    {
        coins.enabled = true;
    }

    private void ChangeScore(int score) => this.score.text = "Score: " + score;
    private void ChangeCoins(int coins) => this.coins.text = "Coins: " + data.lastCoins;
}
