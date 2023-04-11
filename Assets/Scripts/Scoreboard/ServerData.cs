using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerData : MonoBehaviour
{
    #region Get score data

    public void GetScores()
    {
        StartCoroutine(GetScores_Coroutine());
    }

    IEnumerator GetScores_Coroutine()
    {
        string url = "https://defaltuser.000webhostapp.com/menem/?action=get-scores";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);
                //_outputAreaTest.text = webRequest.downloadHandler.text;
            }
            else
            {
                Debug.Log(webRequest.error);
                //_outputAreaTest.text = webRequest.error;
            }
        }
    }

    #endregion

    #region Save score data

    public void SaveScore(int playerScore, string playerName, int gameLevel)
    {
        StartCoroutine(SaveScore_Coroutine(playerScore, playerName, gameLevel));
    }

    IEnumerator SaveScore_Coroutine(int playerScore, string playerName, int gameLevel)
    {
        string url = "https://defaltuser.000webhostapp.com/menem/?action=add-score";
        WWWForm form = new WWWForm();
        form.AddField("name", playerName);
        form.AddField("score", playerScore);
        form.AddField("level", gameLevel);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }

    #endregion
}