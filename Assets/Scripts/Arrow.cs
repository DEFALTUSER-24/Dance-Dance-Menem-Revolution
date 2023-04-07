using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public ArrowKey ID;

    void Start()
    {
        switch (ID)
        {
            case ArrowKey.Up:
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;
            case ArrowKey.Down:
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
            case ArrowKey.Left:
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case ArrowKey.Right:
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
        }

        LeanTween.moveLocal(this.gameObject, GameManager.instance.GetField(), GameManager.instance.delayTime - 0.5f);
    }

    public static Arrow CreateArrow(GameObject arrowPrefab, Transform parent, Vector3 localPosition, ArrowKey id)
    {
        GameObject arrowObject = Instantiate(arrowPrefab);
        RectTransform arrowTransform = arrowObject.GetComponent<RectTransform>();
        Arrow arrowScript = arrowObject.GetComponent<Arrow>();
        arrowTransform.SetParent(parent);
        arrowTransform.localPosition = localPosition;
        arrowScript.ID = id;
        arrowObject.SetActive(false);
        return arrowScript;
    }
}