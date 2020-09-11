using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUIHandler : MonoBehaviour
{
    public TextMeshProUGUI score;

    private void Start()
    {
        GameManager.OnScoreUpdate += UpdateScore;
        score.text = "0";
    }

    private void UpdateScore(int score)
    {
        for (int i = 0; i < score; i++)
        {
            this.score.text = (int.Parse(this.score.text) + 1).ToString();
        }
    }
}
