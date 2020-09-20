using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO.IsolatedStorage;
using UnityEngine.UI;

public class MainMenuUIHandler : MonoBehaviour
{
    public int maxTick = 3;
    private int tickCounter;

    public PlayerData playerData;
    public GameObject dataFrame;

    public GameObject playButtonObject;
    public GameObject playButtonTextObject;
    private TextMeshProUGUI playButtonText;

    public GameObject upgradeButtonObject;
    public GameObject upgradeButtonTextObject;

    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI coinsText;


    private void Start()
    {
        GameManager.OnTick += AnimatePlayText;
        LoadPlayerData();
        playButtonText = playButtonTextObject.GetComponent<TextMeshProUGUI>();
    }

    private int animationIndex;
    private string[] playAimationStates = { "PLAY", "PLAY >", "PLAY >>", "PLAY >>>" };
    private void AnimatePlayText()
    {
        if (tickCounter++ % maxTick == 0)
        {
            tickCounter = 0;
            playButtonText.text = playAimationStates[animationIndex++];
            if (animationIndex > 3)
                animationIndex = 0;
        }
    }

    private void LoadPlayerData()
    {
        ChangeHighscore(playerData.highscore.ToString());
        ChangeCoins(playerData.coins.ToString());

    }

    private void ChangeHighscore(string highscore)
    {
        highscoreText.text = highscore;
    }
    
    private void ChangeCoins(string money)
    {
        coinsText.text = money;
    }

    private void DisablePlayButton()
    {
        playButtonObject.GetComponent<Image>().enabled = false;
        playButtonObject.GetComponent<Button>().enabled = false;

        playButtonText.enabled = false;
    }

    private void disableUpgradeButton()
    {
        upgradeButtonObject.GetComponent<Image>().enabled = false;
        upgradeButtonObject.GetComponent<Button>().enabled = false;

        upgradeButtonTextObject.GetComponent<TextMeshProUGUI>().enabled = false;
    }

    private void DisableDataFrame()
    {
        dataFrame.SetActive(false);
    }


    public void OpenUpgradeMenu()
    {
        DisablePlayButton();
        DisableDataFrame();
        upgradeButtonTextObject.GetComponent<TextMeshProUGUI>().enabled = false;
        LeanTween.scale(upgradeButtonObject, Vector3.one * 50, 1f);
    }

}
