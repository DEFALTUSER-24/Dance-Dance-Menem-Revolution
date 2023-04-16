using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    [Header("Image Feedback")]
                                    private     Image       img;
    [SerializeField] [Range(0, 1f)] private     float       _keyUpAlpha;
    [SerializeField] [Range(0, 1f)] private     float       _keyDownAlpha;
                                    private     KeyCode     keyPressed;
    [Header("Error Margin")]
                                    public      float       errorMargin;

    private HashSet<KeyCode> validKeyCodes;
    private Dictionary<ArrowKey, KeyCode> arrowKeyToKeyCodeMap;

    private const float excellentThreshold = 0.1f;
    private const float veryGoodThreshold = 0.5f;

    private void Start()
    {
        validKeyCodes = new HashSet<KeyCode> { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
        arrowKeyToKeyCodeMap = new Dictionary<ArrowKey, KeyCode> {
            { ArrowKey.Up, KeyCode.UpArrow },
            { ArrowKey.Down, KeyCode.DownArrow },
            { ArrowKey.Left, KeyCode.LeftArrow },
            { ArrowKey.Right, KeyCode.RightArrow }
        };

        img = GetComponent<Image>();
        ChangeOpacity(_keyUpAlpha);
    }

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
                KeypressResult(keyPressed == arrowKeyToKeyCodeMap[GameManager.instance.currentArrow.ID]);
            }
            else
            {
                KeypressResult(false);
            }
        }
        else if (b > (GameManager.instance.currentTimeEvent + errorMargin) * 1.1f && keyPressed == KeyCode.None)
        {
            KeypressResult(false);
        }
    }

    private void DetectKeyPressed()
    {
        keyPressed = KeyCode.None;

        foreach (KeyCode kcode in validKeyCodes)
        {
            if (Input.GetKeyDown(kcode))
            {
                ChangeOpacity(_keyDownAlpha);
                keyPressed = kcode;
                break;
            } else if (Input.GetKeyUp(kcode)) ChangeOpacity(_keyUpAlpha);
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

        float diff = Math.Abs(Mathf.Round((GameManager.instance.currentTimer - GameManager.instance.currentTimeEvent) * 100f) / 100f);

        switch (diff)
        {
            case float d when d <= excellentThreshold:
                GameManager.instance.Score().Update(KeypressPrecision.Excellent);
                break;
            case float d when d <= veryGoodThreshold:
                GameManager.instance.Score().Update(KeypressPrecision.VeryGood);
                break;
            default:
                GameManager.instance.Score().Update(KeypressPrecision.Good);
                break;
        }

        GameManager.instance.DestroyCurrentKey();
    }

    private void ChangeOpacity(float i)
    {
        var tempColor = img.color;
        tempColor.a = i;
        img.color = tempColor;
    }
}