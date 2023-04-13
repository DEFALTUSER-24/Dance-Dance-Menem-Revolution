using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerRequest : MonoBehaviour
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
                UI.instance.UpdateScoreboard(webRequest.downloadHandler.text);
            }
            else
            {
                UI.instance.ShowScoreboardServerError();
            }
        }
    }

    #endregion

    #region Save score data

    //public void SaveScore(int playerScore, string playerName, int gameLevel)
    public void SaveScore()
    {
        //StartCoroutine(SaveScore_Coroutine(playerScore, playerName, gameLevel));
        StartCoroutine(SaveScore_Coroutine());
    }

    //IEnumerator SaveScore_Coroutine(int playerScore, string playerName, int gameLevel)
    IEnumerator SaveScore_Coroutine()
    {
        string url = "https://defaltuser.000webhostapp.com/menem/?action=add-score";
        WWWForm form = new WWWForm();
        //form.AddField("name", playerName);
        //form.AddField("score", playerScore);
        //form.AddField("level", gameLevel);

        form.AddField("name", "asd");
        form.AddField("score", Random.Range(100000, 10000000));
        form.AddField("level", 1);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                UI.instance.UpdateScoreboard(request.downloadHandler.text);
            }
            else
            {
                UI.instance.ShowScoreboardServerError();
            }
        }
    }

    #endregion
}
