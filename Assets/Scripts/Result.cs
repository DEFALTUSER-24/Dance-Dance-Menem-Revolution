using System.Collections;
using UnityEngine;
using TMPro;

public class Result : MonoBehaviour
{
                        public  int                 key;
    [SerializeField]    private TextMeshProUGUI     _text;

    private void KeyResult(KeypressPrecision precision)
    {
        key = (int)precision;

        switch (precision)
        {
            case KeypressPrecision.Excellent:
                _text.color = new Color(0, 255, 0); //green
                break;
            case KeypressPrecision.VeryGood:
                _text.color = new Color(255, 0, 255); //yellow
                break;
            case KeypressPrecision.Good:
                _text.color = new Color(255, 255, 255); //white
                break;
            case KeypressPrecision.Bad:
                _text.color = new Color(255, 0, 0); //red
                break;
        }
        
        StartCoroutine(HideKeypressResultCoroutine());
    }

    IEnumerator HideKeypressResultCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        KeyResult(KeypressPrecision.None);
    }

    public void UpdateKeyPrecision(KeypressPrecision precision)
    {
        KeyResult(precision);
    }
}