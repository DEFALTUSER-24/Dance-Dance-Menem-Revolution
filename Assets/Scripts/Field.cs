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
    public float error;

    private void Update()
    {
        if (!arrow && GameManager.instance.arrows.Count > 0) arrow = GameManager.instance.arrows.Dequeue();
        if (time == 0 && GameManager.instance.seconds.Count > 0) time = GameManager.instance.seconds.Dequeue();

        if (time> GameManager.instance.timer - error && time < GameManager.instance.timer + error)
        {
            switch (arrow.ID)
            {
                case ArrowKey.Up:
                    if (keyPressed == KeyCode.UpArrow)
                    {
                        Debug.Log("Cool");
                    }
                    else
                    {
                        Destroy(arrow);
                        Debug.Log("Mal" + GameManager.instance.timer);
                    }
                    break;
                case ArrowKey.Down:
                    if (keyPressed == KeyCode.UpArrow)
                    {
                            Debug.Log("Cool");

                    }
                    else
                    {
                        Destroy(arrow);
                        Debug.Log("Mal" + GameManager.instance.timer);
                    }
                    break;
                case ArrowKey.Left:
                    if (keyPressed == KeyCode.UpArrow)
                    {
                            Debug.Log("Cool");

                    }
                    else
                    {
                        Destroy(arrow);
                        Debug.Log("Mal" + GameManager.instance.timer);
                    }
                    break;
                case ArrowKey.Right:
                    if (keyPressed == KeyCode.UpArrow)
                    {
                            Debug.Log("Cool");

                    }
                    else
                    {
                        Destroy(arrow);
                        Debug.Log("Mal" + GameManager.instance.timer);
                    }
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
}