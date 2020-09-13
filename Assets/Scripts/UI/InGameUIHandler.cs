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
    }

    private void UpdateScore(int score)
    {
        this.score.text = "Score: " + score;
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
