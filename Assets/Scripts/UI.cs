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

    [Header("Start")]
    [SerializeField] private GameObject     startPanel;

    [Header("In-game")]
    [SerializeField] private GameObject     inGamePanel;
    [SerializeField] private TMP_Text       gameScore;
    [SerializeField] private GameObject     backgroundCanvas;

    [Header("Scoreboard")]
    [SerializeField] private TMP_InputField scoreboard;

    [Header("Game Over")]
    [SerializeField] private GameObject     gameOverPanel;
    [SerializeField] private TMP_Text       finalScore;
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
                ShowStartMenu();
                break;
            case UIInitialPanel.Scoreboard:
                ServerRequest.instance.GetScores();
                break;
        }
    }

    #region Game Score

    public void UpdateGameScore()
    {
        gameScore.text = "Puntaje: " + GameManager.instance.Score().Get();
    }

    private void UpdateFinalScore()
    {
        finalScore.text = "Tu deuda: " + GameManager.instance.Score().Get();
    }

    #endregion

    #region Scoreboard actions

    public void UpdateScoreboard(string json_data)
    {
        try
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
                scoreboard.text = scoreboard.text + "#" + posInBoard.ToString().PadLeft(2, '0') + " | " + data.name + " | Puntaje: " + data.user_score + "\n";
                posInBoard++;
            }
        }
        catch (System.Exception)
        {
            ShowScoreboardError();
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
            ShowScoreUploadError("Tenés que escribir un nombre, máximo 15 letras.");
            return;
        }

        if (scoreUploadInput.text.Trim() == "")
        {
            ShowScoreUploadError("No trates de subir el dolar... poné una letra al menos.");
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
        UpdateFinalScore();

        startPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        inGamePanel.SetActive(false);
        backgroundCanvas.SetActive(false);
    }
    
    public void ShowStartMenu()
    {
        startPanel.SetActive(true);
        inGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        backgroundCanvas.SetActive(false);
        StartCoroutine(ShowInGameUI());
    }

    IEnumerator ShowInGameUI()
    {
        yield return new WaitForSeconds(GameManager.instance.beginLevelTime);
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        backgroundCanvas.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}