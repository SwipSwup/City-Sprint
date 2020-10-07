using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.Networking;

[ExecuteInEditMode]
public class MySQLController : MonoBehaviour
{
    private string secretKey = "Gs82ntnfw98HD93btj2nf92rnIJJBHJVojsDavidSt1nktdns134GD3K0NR4D1U5hallo"; // Edit this value and make sure it's the same as the one stored on the server
    private string addScoreURL = "http://kdender.com/CitySprint/addscore.php?"; //be sure to add a ? to your url
    private string getScoreURL = "http://kdender.com/CitySprint/getScore.php?";
    private string topHighscoresURL = "http://kdender.com/CitySprint/display.php";

    void Start()
    {
        //StartCoroutine(GetScores());
    }

    public void testMethod()
    {
        Debug.Log("testMethod()");

        StartCoroutine(AddScore("email1", "name1", 1));
    }
    
    // remember to use StartCoroutine when calling this function!
    public IEnumerator AddScore(string email, string displayName, int score)
    {
        

        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = MD5Hash(email + displayName + score + secretKey);

        string post_url = addScoreURL + "email=" + UnityWebRequest.EscapeURL(email) + 
            "&displayName=" + UnityWebRequest.EscapeURL(displayName) + "&score=" + score + "&hash=" + hash;

        Debug.Log(post_url);

        // Post the URL to the site and create a download object to get the result.
        UnityWebRequest hs_post = new UnityWebRequest(post_url);
        hs_post.SendWebRequest();

        yield return hs_post; // Wait until the download is done

        if (hs_post.error != null)
        {
            Debug.Log("There was an error posting the high score: " + hs_post.error);
        }
        else
        {
            Debug.Log("----");
            Debug.Log(hs_post);
            Debug.Log(hs_post.GetResponseHeaders());
            Debug.Log("----");
        }
    }

    public int GetScore(string email)
    {
        string hash = MD5Hash(email + secretKey);
        string post_url = getScoreURL + "email=" + UnityWebRequest.EscapeURL(email) + "&hash=" + hash;
        
        UnityWebRequest hs_post = new UnityWebRequest(post_url);
        //yield return hs_post;

        if (hs_post.error != null)
        {
            return -1;
        }

        return 1;
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