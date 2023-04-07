using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    private Image img;
    private Arrow arrow;
    private KeyCode keyPressed;
    private float time;
    public float errorMargin;

    private void Update()
    {
        if (GameManager.instance.arrows.Count == 0 && GameManager.instance.seconds.Count == 0) return;

        if (!arrow) arrow = GameManager.instance.arrows.Dequeue();
        if (time == 0) time = GameManager.instance.seconds.Dequeue();

        float v = GameManager.instance.timer - errorMargin;
        float b = GameManager.instance.timer + errorMargin;

        if (v < time && b > time)
        {
            Debug.Log("Ya!");
            switch (arrow.ID)
            {
                case ArrowKey.Up:
                    if (keyPressed == KeyCode.UpArrow)
                    {
                        Debug.Log("Cool");
                    }
                    else Reset();
                    break;
                case ArrowKey.Down:
                    if (keyPressed == KeyCode.UpArrow)
                    {
                        Debug.Log("Cool");
                    }
                    else Reset();
                    break;
                case ArrowKey.Left:
                    if (keyPressed == KeyCode.UpArrow)
                    {
                        Debug.Log("Cool");

                    }
                    else Reset();
                    break;
                case ArrowKey.Right:
                    if (keyPressed == KeyCode.UpArrow)
                    {
                        Debug.Log("Cool");
                    }
                    else Reset();
                    break;
            }

        }
    }

    public void DetectKeyPressed()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                keyPressed = kcode;
            }
        }
    }

    private void Reset()
    {
        Destroy(arrow.gameObject);
        time = 0;
        arrow = null;
        Debug.Log("Mal" + GameManager.instance.timer);
    }
}