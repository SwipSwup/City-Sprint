using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO.IsolatedStorage;

public class MainMenuUIHandler : MonoBehaviour
{
    public int maxTick = 3;
    private int tickCounter;

    public GameObject playButtonObject;
    public TextMeshProUGUI playButtonText;

    public GameObject upgradeButtonObject;
    public GameObject upTextObject;


    private void OnEnable()
    {
    }

    private void Start()
    {
        GameManager.OnTick += AnimatePlayText;
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

    public void OpenUpgradeMenu()
    {
        LeanTween.scale(upgradeButtonObject, Vector3.one * 50, .5f);
        playButtonObject.active = false;
        upTextObject.active = false;
    }

}
