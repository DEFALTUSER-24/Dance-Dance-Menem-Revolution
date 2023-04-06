using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public ArrowKey ID;
    void Start()
    {
        LeanTween.moveLocal(this.gameObject, GameManager.instance.GetField(), GameManager.instance.delayTime);
    }
}