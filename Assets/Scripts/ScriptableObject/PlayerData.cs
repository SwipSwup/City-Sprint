using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{

    public string email = null;
    public string displayName = "annonym";

    public bool firsTimeStart = true;

    public int place = 0;
    public int lastScore = 0;
    public int highscore = 0;
    public int lastCoins = 0;
    public int coins = 0;
}
