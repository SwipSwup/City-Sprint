using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.Networking;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MySQLController : MonoBehaviour
{
    private static string secretKey = "Gs82ntnfw98HD93btj2nf92rnIJJBHJVojsDavidSt1nktdns134GD3K0NR4D1U5hallo";
    private static string addScoreURL = "http://kdender.com/CitySprint/addscore.php";
    private static string getScoreURL = "http://kdender.com/CitySprint/getscore.php";
    private static string getTopHighscoresURL = "http://kdender.com/CitySprint/gettophighscores.php";

    public void testMethod(string email, string name, int i)
    {

        StartCoroutine(AddScore("david.stinkt@kannNichtProgrammieren.at", "testName", 5));

        //Debug.Log(GetScore(email));

        //ArrayList test = GetTopHighscores();
        //string log = "";
        //foreach (KeyValuePair<string, int> kvp in test)
        //{
        //    log += kvp.Key + " | " + kvp.Value + "\n";
        //}
        //Debug.Log(log);
    }

    // updates score on database if its higher than the last one and inserts it if it doesn't exist yet
    // StartCoroutine(AddScore(email of player, name choosen by the player, new highscore the player reached))
    public static IEnumerator AddScore(string email, string displayName, int score)
    {
        string hash = MD5Hash(email + displayName + score + secretKey);
        string post_url = addScoreURL + "?email=" + UnityWebRequest.EscapeURL(email) +
            "&displayName=" + UnityWebRequest.EscapeURL(displayName) + "&score=" + score + "&hash=" + hash;

        UnityWebRequest hs_post = new UnityWebRequest(post_url);
        yield return hs_post.SendWebRequest();
        while (!hs_post.isDone) ;

        if (hs_post.isNetworkError)
        {
            Debug.LogError("Error: " + hs_post.error);
        }
        else if (hs_post.downloadHandler != null)
        {
            string response = hs_post.downloadHandler.text;
            if (response != null && response != "") Debug.LogError(response);
        }
    }

    // returns the score of the player with the email (string email)
    public static int GetScore(string email)
    {
        string hash = MD5Hash(email + secretKey);
        string post_url = getScoreURL + "?email=" + UnityWebRequest.EscapeURL(email) + "&hash=" + hash;
        Debug.Log(post_url);

        UnityWebRequest hs_post = UnityWebRequest.Get(post_url);
        hs_post.SendWebRequest();
        while (!hs_post.isDone) ;

        if (hs_post.isNetworkError)
        {
            Debug.LogError("Error: " + hs_post.error);
            return -1;
        }
        else if (int.TryParse(hs_post.downloadHandler.text, out int score))
        {
            return score;
        }
        return 0;
    }
    
    // returns a List containing KeyValuePair<string, int> containing the top 100 Highscores where string=displayName and int=score
    public static ArrayList GetTopHighscores()
    {
        UnityWebRequest hs_post = UnityWebRequest.Get(getTopHighscoresURL);
        hs_post.SendWebRequest();
        while (!hs_post.isDone) ;

        ArrayList scoreList = new ArrayList();
        if (hs_post.isNetworkError)
        {
            Debug.LogError("Error: " + hs_post.error);
            scoreList.Add(new KeyValuePair<string, int>("Error", 0));
            return scoreList;
        }
        string result = hs_post.downloadHandler.text;
        if (result == null || result == "")
        {
            scoreList.Add(new KeyValuePair<string, int>("", 0));
            return scoreList;
        }

        string[] array;
        int n;
        foreach (string s in result.Split('\n'))
        {
            array = s.Split('#');
            if (s != "" && s != null && s != "\n" && int.TryParse(array[1], out n))
            {
                scoreList.Add(new KeyValuePair<string, int>(array[0], n));
            }
        }
        return scoreList;
    }

    // calculates md5 hash
    public static string MD5Hash(string input)
    {
        StringBuilder hash = new StringBuilder();
        MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

        for (int i = 0; i < bytes.Length; i++)
        {
            hash.Append(bytes[i].ToString("x2"));
        }
        return hash.ToString();
    }
}