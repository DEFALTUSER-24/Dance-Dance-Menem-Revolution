using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class ScoreDescription : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_InputField         _scoreUploadInput;
    [SerializeField] private TMP_Text               _serverErrorText;
    [SerializeField] private LocalizeStringEvent    _localizeStringEvent;

    [Header("Mesagges")]
    [SerializeField] private LocalizedString        _successfulMessage;
    [SerializeField] private LocalizedString        _nameIsNull;
    [SerializeField] private LocalizedString        _nameHaveMuchBlankSpace;
    [SerializeField] private LocalizedString        _errorMessage;

    public bool ShowScoreUploadError()
    {
        if(_scoreUploadInput.text != "")
        {
            _localizeStringEvent.StringReference = _successfulMessage;
            _scoreUploadInput.gameObject.SetActive(false);
            return true;
        }
        else if(_scoreUploadInput.text == "")
        {
            _localizeStringEvent.StringReference =  _nameIsNull;
            return false;
        }
        else if (_scoreUploadInput.text.Trim() == "")
        {
            _localizeStringEvent.StringReference = _nameHaveMuchBlankSpace;
            return false;
        }
        else
        {
            return false;
        }
    }

    public string GetPlayerName()
    {
        _serverErrorText.text = "";
        return _scoreUploadInput.text;
    }
}