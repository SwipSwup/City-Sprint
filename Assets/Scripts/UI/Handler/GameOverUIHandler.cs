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

        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < data.lastScore; i += data.lastScore / 20)
        {
            ChangeScore(i);
            yield return new WaitForSeconds(.05f);
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
