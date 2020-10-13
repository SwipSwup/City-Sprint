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

    public static Action OnPlay;
    public void Play()
    {
        OnPlay?.Invoke();
        DisableMenuUI();
        GetComponent<InGameUIHandler>().EnableInGameUI();
    }

    private void DisableMenuUI() => mainMenuCanvas.enabled = false;

    private void EnableMenuUI() => mainMenuCanvas.enabled = true;
}
