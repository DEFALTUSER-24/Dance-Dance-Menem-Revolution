using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerRequest : MonoBehaviour
{
    public static ServerRequest instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

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
                UI.instance.ShowScoreboardError();
            }
        }
    }

    #endregion

    #region Save score data

    //public void SaveScore(int playerScore, string playerName, int gameLevel)
    public void SaveScore(int playerScore, string playerName)
    {
        int gameLevel = 1; //Cambiar esto cuando se agreguen mas niveles
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
                UI.instance.OnScoreSaved();
            }
            else
            {
                UI.instance.ShowScoreUploadError();
            }
        }
    }

    #endregion
}
