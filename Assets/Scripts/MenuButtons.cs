using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject _buttonsPanel;
    [SerializeField] private GameObject _howToPlayPanel;
    [SerializeField] private GameObject _creditsPanel;

    [Header("Botones de volver atrás")]
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _backButtonHowToPlayPanel;
    [SerializeField] private GameObject _backButtonCreditsPanel;

    [Header("Escenas")]
    [SerializeField] private int _gameScene;
    [SerializeField] private int _scoreScene;

    private void Start()
    {
        Back();
    }

    public void BeginPlay()
    {
        SceneManager.LoadScene(_gameScene);
    }

    public void HowToPlay()
    {
        _buttonsPanel.SetActive(false);
        _howToPlayPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_backButtonHowToPlayPanel);
    }

    public void Credits()
    {
        _buttonsPanel.SetActive(false);
        _creditsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_backButtonCreditsPanel);
    }

    public void Ranking()
    {
        SceneManager.LoadScene(_scoreScene);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Back()
    {
        _howToPlayPanel.SetActive(false);
        _creditsPanel.SetActive(false);
        _buttonsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_startButton);
    }
}