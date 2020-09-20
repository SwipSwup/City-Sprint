using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUIHandler : MonoBehaviour
{
    [SerializeField] private Image expandCircle;
    [SerializeField] private Vector3 circleSize;
    [SerializeField] private LeanTweenType easeType;

    [SerializeField] private Canvas GameOverCanvas;

    private void Start()
    {
        Player.OnGameOver += OpenGameOverMenu;
    }

    private void OpenGameOverMenu()
    {
    }
}
