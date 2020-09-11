using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUIHandler : MonoBehaviour
{
    public TextMeshProUGUI score;


    private void Start()
    {
        
    }

    private void UpdateScore(int score)
    {
        this.score.text = score.ToString();
    }
}
