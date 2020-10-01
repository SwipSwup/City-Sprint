using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int place;
    public string playerName;

    public int lastScore;
    public int highscore;
    public int lastCoins;
    public int coins;
}
