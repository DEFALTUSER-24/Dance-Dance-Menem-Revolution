using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    public ArrowKey ID;
    [Header("Components")]
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _transform;
    [Header("Positions")]
    [SerializeField] private Vector3 _initialPos;
    [SerializeField] private Vector3 _fieldPos;
    [SerializeField] private Vector3 _endPos;
    [Header("Colors")]
    [SerializeField] private Color _upColor;
    [SerializeField] private Color _downColor;
    [SerializeField] private Color _leftColor;
    [SerializeField] private Color _rightColor;

    void Start()
    {
        _transform.GetComponent<RectTransform>().localPosition = _initialPos;
        _fieldPos = new Vector3(GameManager.instance.GetField().x, GameManager.instance.GetField().y, 0);

        switch (ID)
        {
            case ArrowKey.Up:
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                _image.color = _upColor;
                break;
            case ArrowKey.Down:
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                _image.color = _downColor;
                break;
            case ArrowKey.Left:
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                _image.color = _leftColor;
                break;
            case ArrowKey.Right:
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                _image.color = _rightColor;
                break;
        }

        float d1 = Vector3.Distance(_initialPos, _fieldPos);
        float d2 = Vector3.Distance(_fieldPos, _endPos);
        float t = GameManager.instance.delayTime * d2 / d1;

        LeanTween.moveLocal(this.gameObject, _endPos, GameManager.instance.delayTime + t);
    }

    public void DestroyMe(bool wasGood, bool wasPressed)
    {
        if ((!wasGood && wasPressed) || (wasGood && wasPressed)) Destroy(this.gameObject);
        else
        {
            float d1 = Vector3.Distance(_initialPos, _fieldPos);
            float d2 = Vector3.Distance(_fieldPos, _endPos);
            float t = GameManager.instance.delayTime * d2 / d1;
            Destroy(this.gameObject, t);
        }
    }

    public static Arrow CreateArrow(GameObject arrowPrefab, Transform parent, ArrowKey id)
    {
        GameObject arrowObject = Instantiate(arrowPrefab);
        RectTransform arrowTransform = arrowObject.GetComponent<RectTransform>();
        Arrow arrowScript = arrowObject.GetComponent<Arrow>();
        arrowTransform.SetParent(parent);
        arrowTransform.localScale = new Vector3(1, 1, 1);
        arrowTransform.localPosition = new Vector3(450, -150, 0);
        arrowScript.ID = id;
        arrowObject.SetActive(false);
        return arrowScript;
    }
}