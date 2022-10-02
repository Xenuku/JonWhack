using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
// I used the following answers to help guide the development of this scene
// https://stackoverflow.com/questions/46003824/sending-http-requests-in-c-sharp-with-unity
public class HighScores : MonoBehaviour
{

    public TMP_Text highScoresText;
    void Start()
    {
        StartCoroutine(getScores("http://dreamlo.com/lb/63399d828f40bc0fe88fdf01/pipe/10"));
    }

    IEnumerator getScores(string uri) 
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error while getting: " + uwr.error);
        } 
        else
        {
            //Debug.Log("Recieved: " + uwr.downloadHandler.text);
            string[] entries = uwr.downloadHandler.text.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
            string textBuilder = "";
            for (int i = 0; i < entries.Length; i++) {
                string[] entryInfo = entries[i].Split(new char[] {'|'});
                int positon = i + 1;
                textBuilder += "" + positon + ". ";
                string username = entryInfo[0];
                textBuilder += username + " - ";
                string survived = entryInfo[3];
                textBuilder += survived + " - ";
                int score = int.Parse(entryInfo[1]);
                textBuilder += score.ToString("#,#") + "\n\n";         
            }
            highScoresText.text = textBuilder;
        }
    }
}