using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class Result : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]    private TextMeshProUGUI     _text;
    [SerializeField]    private LocalizeStringEvent _lse;
    [Header("Results Strings")]
    [SerializeField]    private LocalizedString     _perfect;
    [SerializeField]    private LocalizedString     _veryGood;
    [SerializeField]    private LocalizedString     _Good;
    [SerializeField]    private LocalizedString     _bad;
    
                        private bool                _clear;

    public void KeyResult(KeypressPrecision precision)
    {
        switch (precision)
        {
            case KeypressPrecision.Excellent:
                _lse.StringReference = _perfect;
                _text.color = new Color(0, 255, 0); //green
                break;
            case KeypressPrecision.VeryGood:
                _lse.StringReference = _veryGood;
                _text.color = new Color(255, 0, 255); //yellow
                break;
            case KeypressPrecision.Good:
                _lse.StringReference = _Good;
                _text.color = new Color(255, 255, 255); //white
                break;
            case KeypressPrecision.Bad:
                _lse.StringReference = _bad;
                _text.color = new Color(255, 0, 0); //red
                break;
        }
        
        if(!_clear) StartCoroutine(HideKeypressResultCoroutine());
    }

    IEnumerator HideKeypressResultCoroutine()
    {
        _clear = true;
        yield return new WaitForSeconds(1.5f);
        _text.text = "";
        _clear = false;
    }
}