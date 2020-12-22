using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI place;
    [SerializeField] private new TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI score;

    public void SetEntryData(int place, string name, string score)
    {
        this.place.text = "#" + Mathf.Clamp(place, 1, 999) + (place > 999 ? "+" : "");
        this.name.text = name;
        this.score.text = score;
    }
}
