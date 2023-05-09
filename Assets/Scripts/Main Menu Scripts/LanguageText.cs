using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LanguageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI    _textComponent;
    [SerializeField] private string             _languageSelection;

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject) _textComponent.text = _languageSelection;
    }
}