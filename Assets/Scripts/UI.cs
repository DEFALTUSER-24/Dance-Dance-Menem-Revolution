using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    public static UI instance;

    [Header("EventSystem")]
    [SerializeField] private GameObject     _gameOverButton;
    [SerializeField] private GameObject     _pauseButton;

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
    [SerializeField] private GameObject     arrowsCanvas;
    [SerializeField] private TMP_Text       keypressResult;

    [Header("Pause menu")]
    [SerializeField] private GameObject     pauseMenuPanel;

    [Header("Scoreboard")]
    [SerializeField] private TMP_InputField scoreboard;

    [Header("Game Over")]
    [SerializeField] private GameObject     gameOverPanel;
    [SerializeField] private TMP_Text       finalScore;
    [SerializeField] private TMP_InputField scoreUploadInput;
    [SerializeField] private TMP_Text       serverErrorText;
    [SerializeField] private GameObject     uploadScoreButton;
    [SerializeField] private GameObject     writeYourNameText;

    private Coroutine keypressResultCoroutine;

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

        if (keypressResult != null)
            keypressResult.text = "";
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

    public void UpdateKeypressResult(KeypressPrecision precision)
    {
        switch (precision)
        {
            case KeypressPrecision.Excellent:
                keypressResult.text = "Excelente";
                keypressResult.color = new Color(0, 255, 0); //green
                break;
            case KeypressPrecision.VeryGood:
                keypressResult.text = "Muy bien";
                keypressResult.color = new Color(255, 0, 255); //yellow
                break;
            case KeypressPrecision.Good:
                keypressResult.text = "Bien";
                keypressResult.color = new Color(255, 255, 255); //white
                break;
            case KeypressPrecision.Bad:
                keypressResult.text = "Malisimo";
                keypressResult.color = new Color(255, 0, 0); //red
                break;
        }

        if (keypressResultCoroutine != null)
            StopCoroutine(keypressResultCoroutine);

        keypressResultCoroutine = StartCoroutine(HideKeypressResultCoroutine());
    }

    IEnumerator HideKeypressResultCoroutine()
    {
        yield return new WaitForSeconds(1);
        keypressResult.text = "";
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
        arrowsCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_gameOverButton);
        GameManager.instance.SwitchActionsMaps();
    }
    
    public void ShowStartMenu()
    {
        GameManager.instance.currentGameState = GameState.Start;
        startPanel.SetActive(true);
        inGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        backgroundCanvas.SetActive(false);
        arrowsCanvas.SetActive(false);
        StartCoroutine(ShowInGameUI());
    }

    IEnumerator ShowInGameUI()
    {
        yield return new WaitForSeconds(GameManager.instance.beginLevelTime);
        GameManager.instance.currentGameState = GameState.Game;
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        backgroundCanvas.SetActive(true);
        arrowsCanvas.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.SwitchActionsMaps();
    }

    public void TogglePauseMenu()
    {
        bool isActive = !pauseMenuPanel.activeSelf;
        pauseMenuPanel.SetActive(isActive);
        EventSystem.current.SetSelectedGameObject(_pauseButton);

        if(GameManager.instance.currentGameState == GameState.Game)
        {
            inGamePanel.SetActive(!isActive);
            backgroundCanvas.SetActive(!isActive);
            arrowsCanvas.SetActive(!isActive);
            startPanel.SetActive(false);
        }
        else if(GameManager.instance.currentGameState == GameState.Start)
        {
            startPanel.SetActive(!isActive);
            inGamePanel.SetActive(false);
            backgroundCanvas.SetActive(false);
            arrowsCanvas.SetActive(false);
        }
    }

    public void Resume()
    {
        TogglePauseMenu();
        GameManager.instance.ToggleGamePause();
        GameManager.instance.SwitchActionsMaps();
    }

    #endregion
}