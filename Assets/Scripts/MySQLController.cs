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
    private static string addScoreURL = "http://kdender.com/CitySprint/addscore.php?"; //be sure to add a ? to your url
    private static string getScoreURL = "http://kdender.com/CitySprint/getscore.php?";
    private static string topHighscoresURL = "http://kdender.com/CitySprint/display.php";

    public void testMethod(string email, string name, int i)
    {

        StartCoroutine(AddScore(email, "testName", i));

        //Debug.Log(GetScore(email));
    }
 
    
    // updates score on database if its higher than the last one and inserts it if it doesn't exist yet
    // StartCoroutine(AddScore(email of player, name choosen by the player, new highscore the player reached))
    public static IEnumerator AddScore(string email, string displayName, int score)
    {
        string hash = MD5Hash(email + displayName + score + secretKey);
        hash = email;
        string post_url = addScoreURL + "email=" + UnityWebRequest.EscapeURL(email) + 
            "&displayName=" + UnityWebRequest.EscapeURL(displayName) + "&score=" + score + "&hash=" + hash;
        
        UnityWebRequest hs_post = new UnityWebRequest(post_url);
        yield return hs_post.SendWebRequest();

        if (hs_post.isNetworkError)
        {
            Debug.LogError("Error: " + hs_post.error);
        }
        else
        {
            string response = hs_post.downloadHandler.text;
            if (response != null && response != "")
            {
                Debug.LogError(response);
            }
        }
    }

    public int GetScore(string email)
    {
        string hash = MD5Hash(email + secretKey);
        string post_url = getScoreURL + "email=" + UnityWebRequest.EscapeURL(email) + "&hash=" + hash;

        UnityWebRequest hs_post = UnityWebRequest.Get(post_url);
        hs_post.SendWebRequest();
        
        if (hs_post.isNetworkError)
        {
            Debug.LogError("Error: " + hs_post.error);
            return -1;
        }
        else if (int.TryParse(hs_post.downloadHandler.text, out int score))
        {
            return score;
        }
        Debug.Log(hs_post.downloadHandler.text);
        Debug.Log("No score in data base");
        return 0;
    }

    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    IEnumerator GetTopHighscores()
    {
        //gameObject.guiText.text = "Loading Scores";
        UnityWebRequest hs_get = new UnityWebRequest(topHighscoresURL);
        yield return hs_get;

        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            //gameObject.guiText.text = hs_get.text; // this is a GUIText that will display the scores in game.
        }
    }

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