using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField] private TMP_InputField scoreboard;
    [SerializeField] private Text gameScore;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void UpdateGameScore()
    {
        Debug.Log(GameManager.instance.Score().Get());
    }

    public void UpdateScoreboard(string json_data)
    {
        ServerScoreboardData[] json = JsonHelper.FromJson<ServerScoreboardData>(json_data);

        if (json.Length == 0 || json == null)
        {
            scoreboard.text = "Error al obtener los datos del servidor";
            return;
        }

        int posInBoard = 1;

        foreach (ServerScoreboardData data in json)
        {
            scoreboard.text = scoreboard.text + "#" + posInBoard.ToString().PadLeft(2, '0') + " | " + data.name + " | Puntaje: " + data.user_score + " - Level: " + data.level + "\n";
            posInBoard++;
        }
    }

    public void ShowScoreboardServerError()
    {

    }
}