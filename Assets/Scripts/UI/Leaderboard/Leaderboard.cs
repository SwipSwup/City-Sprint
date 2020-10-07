using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardEntryField;
    [SerializeField] private GameObject leaderboardEntryPrefab;
    [SerializeField] private int maxEntries;

    [SerializeField] private PlayerData playerData;

    private void Start()
    {
        fillLeaderboard();
    }

    private void fillLeaderboard()
    {
        for (int i = 0; i < maxEntries - 1; i++)
        {
            Instantiate(leaderboardEntryPrefab, leaderboardEntryField.transform);
        }    
        GameObject playerEntry = Instantiate(leaderboardEntryPrefab, leaderboardEntryField.transform);
        playerEntry.GetComponent<LeaderboardEntry>().SetEntryData(playerData.place, playerData.displayName, playerData.highscore.ToString());
    }
}
