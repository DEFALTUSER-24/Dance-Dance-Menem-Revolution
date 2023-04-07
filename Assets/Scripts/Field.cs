using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    private Image img;
    private KeyCode keyPressed;
    public float errorMargin;

    private void Update()
    {
        if (GameManager.instance.currentArrow == null && GameManager.instance.currentTimeEvent == 0) return;

        float v = GameManager.instance.currentTimer - errorMargin;
        float b = GameManager.instance.currentTimer + errorMargin;
        DetectKeyPressed();

        if (keyPressed != 0)
        {
            if (v < GameManager.instance.currentTimeEvent && b > GameManager.instance.currentTimeEvent)
            {
                switch (GameManager.instance.currentArrow.ID)
                {
                    case ArrowKey.Up:
                        if (keyPressed == KeyCode.UpArrow)
                        {
                            GoodResult();
                        }
                        else BadResult();
                        break;
                    case ArrowKey.Down:
                        if (keyPressed == KeyCode.DownArrow)
                        {
                            GoodResult();
                        }
                        else BadResult();
                        break;
                    case ArrowKey.Left:
                        if (keyPressed == KeyCode.LeftArrow)
                        {
                            GoodResult();

                        }
                        else BadResult();
                        break;
                    case ArrowKey.Right:
                        if (keyPressed == KeyCode.RightArrow)
                        {
                            GoodResult();
                        }
                        else BadResult();
                        break;
                }
            }
            else BadResult();
        }
        else if (b > (GameManager.instance.currentTimeEvent + errorMargin) * 1.1f && keyPressed == 0)
        {
            BadResult();
            Debug.Log(GameManager.instance.currentArrow.ID);
        }
    }

    public void DetectKeyPressed()
    {
        keyPressed = KeyCode.None;

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode)) keyPressed = kcode;
        }
    }

    private void GoodResult()
    {
        Destroy(GameManager.instance.currentArrow.gameObject);
        GameManager.instance.currentTimeEvent = 0;
        GameManager.instance.currentArrow = null;
        Debug.Log("Cool" + GameManager.instance.currentTimer);
    }

    private void BadResult()
    {
        Destroy(GameManager.instance.currentArrow.gameObject);
        GameManager.instance.currentTimeEvent = 0;
        GameManager.instance.currentArrow = null;
        Debug.Log("Mal");
    }
}