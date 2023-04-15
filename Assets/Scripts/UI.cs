using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI instance;

    [Header("Main menu scene")]
    [SerializeField] private int            mainMenuScene;

    [Header("Select an initial menu")]
    [SerializeField] private UIInitialPanel initialPanel;

    [Header("In-game")]
    [SerializeField] private GameObject     inGamePanel;
    [SerializeField] private TMP_Text       gameScore;

    [Header("Scoreboard")]
    [SerializeField] private TMP_InputField scoreboard;

    [Header("Game Over")]
    [SerializeField] private GameObject     gameOverPanel;
    [SerializeField] private TMP_InputField scoreUploadInput;
    [SerializeField] private TMP_Text       serverErrorText;
    [SerializeField] private GameObject     uploadScoreButton;
    [SerializeField] private GameObject     writeYourNameText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        switch (initialPanel)
        {
            case UIInitialPanel.GameOver:
                ShowGameOverMenu();
                break;
            case UIInitialPanel.InGame:
                ShowInGameMenu();
                break;
            case UIInitialPanel.Scoreboard:
                ServerRequest.instance.GetScores();
                break;
        }
    }
    public void UpdateGameScore()
    {
        gameScore.text = "Puntaje: " + GameManager.instance.Score().Get();
    }

    #region Scoreboard actions

    public void UpdateScoreboard(string json_data)
    {
        if (json_data == "")
        {
            ShowScoreboardError("No hay datos");
            return;
        }

        ServerScoreboardData[] json = JsonHelper.FromJson<ServerScoreboardData>(json_data);

        if (json.Length == 0 || json == null)
        {
            ShowScoreboardError();
            return;
        }

        int posInBoard = 1;

        foreach (ServerScoreboardData data in json)
        {
            scoreboard.text = scoreboard.text + "#" + posInBoard.ToString().PadLeft(2, '0') + " | " + data.name + " | Puntaje: " + data.user_score + " - Level: " + data.level + "\n";
            posInBoard++;
        }
    }

    public void ShowScoreboardError(string error = "Error al obtener los datos del servidor")
    {
        scoreboard.text = error;
    }

    #endregion

    #region Game Over panel actions
    public void ShowScoreUploadError(string errorDescription = "")
    {
        serverErrorText.text = errorDescription != "" ? errorDescription : "Error al subir el puntaje al servidor";
    }

    public void SaveScore()
    {
        if (scoreUploadInput.text == "")
        {
            ShowScoreUploadError("Tenés que escribir un nombre, máximo 24 letras.");
            return;
        }

        GameManager.instance.Score().Save(scoreUploadInput.text);
    }

    public void OnScoreSaved()
    {
        ShowScoreUploadError("Puntaje guardado correctamente.");
        scoreUploadInput.gameObject.SetActive(false);
        uploadScoreButton.SetActive(false);
        writeYourNameText.SetActive(false);

        scoreUploadInput.text = "";
    }

    #endregion

    #region Menu panels/loaders

    public void ShowGameOverMenu()
    {
        gameOverPanel.SetActive(true);
        inGamePanel.SetActive(false);
    }
    
    public void ShowInGameMenu()
    {
        inGamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    #endregion
}