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

        if (keyPressed != KeyCode.None)
        {
            if (v < GameManager.instance.currentTimeEvent && b > GameManager.instance.currentTimeEvent)
            {
                switch (GameManager.instance.currentArrow.ID)
                {
                    case ArrowKey.Up:
                        KeypressResult(keyPressed == KeyCode.UpArrow);
                        break;
                    case ArrowKey.Down:
                        KeypressResult(keyPressed == KeyCode.DownArrow);
                        break;
                    case ArrowKey.Left:
                        KeypressResult(keyPressed == KeyCode.LeftArrow);
                        break;
                    case ArrowKey.Right:
                        KeypressResult(keyPressed == KeyCode.RightArrow);
                        break;
                }
            }
            else KeypressResult(false);
        }
        else if (b > (GameManager.instance.currentTimeEvent + errorMargin) * 1.1f && keyPressed == KeyCode.None)
        {
            KeypressResult(false);
        }
    }

    private void DetectKeyPressed()
    {
        keyPressed = KeyCode.None;

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                if (kcode == KeyCode.DownArrow || kcode == KeyCode.UpArrow ||
                    kcode == KeyCode.LeftArrow || kcode == KeyCode.RightArrow)
                {
                    keyPressed = kcode;
                    break;
                }
            }
        }
    }

    private void KeypressResult(bool wasGood)
    {
        if (!wasGood)
        {
            GameManager.instance.DestroyCurrentKey();
            GameManager.instance.Score().Update(KeypressPrecision.Bad);
            return;
        }

        KeypressPrecision result = KeypressPrecision.Good;

        float diff = Mathf.Round((GameManager.instance.currentTimer - GameManager.instance.currentTimeEvent) * 100f) / 100f;
        diff = diff < 0 ? diff * -1 : diff;

        if (diff <= 0.1f && diff > 0)
        {
            result = KeypressPrecision.Excellent;
        }
        else if (diff <= 0.5f && diff > 0.1f)
        {
            result = KeypressPrecision.VeryGood;
        }
        else if (diff > 0.5f)
        {
            result = KeypressPrecision.Good;
        }

        GameManager.instance.DestroyCurrentKey();
        GameManager.instance.Score().Update(result);
    }
}