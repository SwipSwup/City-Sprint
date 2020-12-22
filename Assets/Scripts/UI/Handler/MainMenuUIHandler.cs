using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO.IsolatedStorage;
using UnityEngine.UI;
using System;

public class MainMenuUIHandler : MonoBehaviour
{
    public Canvas mainMenuCanvas;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject upperBar;

    [SerializeField] private TextMeshProUGUI highscore_value;
    [SerializeField] private TextMeshProUGUI lastScore_value;
    [SerializeField] private TextMeshProUGUI coins_value;


    public void OnEnable()
    {
        SetValues();
    }

    private void SetValues()
    {
        SetHighscoreValue(playerData.highscore);
        SetLastScoreValue(playerData.lastScore);
        SetCoinsValue(playerData.coins);
    }

    public static Action OnPlay;
    public void Play()
    {
        OnPlay?.Invoke();
        DisableMenuUI();
        GetComponent<InGameUIHandler>().EnableInGameUI();
        LeanTween.moveZ(upperBar, 200f, 1);
    }

    private void DisableMenuUI() => mainMenuCanvas.enabled = true;

    private void EnableMenuUI() => mainMenuCanvas.enabled = true;

    private void SetHighscoreValue(int value) => highscore_value.text = value.ToString();
    private void SetLastScoreValue(int value) => lastScore_value.text = value.ToString();
    private void SetCoinsValue(int value) => coins_value.text = value.ToString();
}
