using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    private Image img;
    private Arrow arrow;

    private void Update()
    {
        if (!arrow && GameManager.instance.arrows.Count > 0) arrow = GameManager.instance.arrows.Dequeue();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(arrow.ID);
            Destroy(arrow.gameObject);
        }
    }
}
