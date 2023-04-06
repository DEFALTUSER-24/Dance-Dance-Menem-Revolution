using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject   arrow;
    public GameObject   field;
    public Canvas       canvas;
    public float        delayTime;
    public Queue<Arrow> arrows = new Queue<Arrow>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {

    }

    public void ShowArrow(ArrowKey key)
    {
        GameObject      a               = Instantiate(arrow);
        RectTransform   arrowTransform  = a.GetComponent<RectTransform>();
        Arrow           arrowScript     = a.GetComponent<Arrow>();

        arrowTransform.SetParent(canvas.transform);
        arrowTransform.localPosition = new Vector3(275, -117, 0);

        arrowScript.ID = key;
        arrows.Enqueue(arrowScript);

        switch (key)
        {
            case ArrowKey.Up:
                arrowTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;
            case ArrowKey.Down:
                arrowTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case ArrowKey.Left:
                arrowTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case ArrowKey.Right:
                arrowTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
        }
    }

    public Vector3 GetField()
    {
        return field.GetComponent<RectTransform>().localPosition;
    }
}