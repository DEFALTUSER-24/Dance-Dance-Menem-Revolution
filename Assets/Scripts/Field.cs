using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Field : MonoBehaviour
{
    [Header("Image Feedback")]
    [SerializeField]                private     Image       _backColor;
    [SerializeField] [Range(0, 1f)] private     float       _keyUpAlpha;
    [SerializeField] [Range(0, 1f)] private     float       _keyDownAlpha;
                                    private     ArrowKey     keyPressed;
    [Header("Error Margin")]
                                    public      float       errorMarginBeforeField;
                                    public      float       errorMarginAfterField;

    private const float excellentThreshold = 0.05f;
    private const float veryGoodThreshold = 0.1f;

    private void Start()
    {
        ChangeOpacity(_keyUpAlpha);
    }

    private void Update()
    {
        if ((GameManager.instance.currentArrow == null && GameManager.instance.currentTimeEvent == 0) ||
            GameManager.instance.gamePaused) return;

        float v = GameManager.instance.currentTimer - errorMarginAfterField;
        float b = GameManager.instance.currentTimer + errorMarginBeforeField;
        DetectKeyPressed();

        if (keyPressed != ArrowKey.None)
        {
            if (v < GameManager.instance.currentTimeEvent && b > GameManager.instance.currentTimeEvent)
            {
                KeypressResult(keyPressed == GameManager.instance.currentArrow.ID, true);
            }
            else
            {
                KeypressResult(false, true);
            }
        }
        else if (b > (GameManager.instance.currentTimeEvent + errorMarginAfterField) && keyPressed == ArrowKey.None)
        {
            KeypressResult(false, false);
        }
    }

    private void DetectKeyPressed()
    {
        keyPressed = ArrowKey.None;

        foreach (InputAction action in GameManager.instance.pInput.actions)
        {
            if (GameManager.instance.pInput.actions[action.name].WasPressedThisFrame())
            {
                ChangeOpacity(_keyDownAlpha);
                switch (action.name)
                {
                    case "Up":
                        keyPressed = ArrowKey.Up;
                        break;
                    case "Down":
                        keyPressed = ArrowKey.Down;
                        break;
                    case "Left":
                        keyPressed = ArrowKey.Left;
                        break;
                    case "Right":
                        keyPressed = ArrowKey.Right;
                        break;
                }
                break;
            }
            else if (GameManager.instance.pInput.actions[action.name].WasReleasedThisFrame()) ChangeOpacity(_keyUpAlpha);
        }
    }

    private void KeypressResult(bool wasGood, bool wasPressed)
    {
        if (!wasGood)
        {
            GameManager.instance.DestroyCurrentKey(wasGood, wasPressed);
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

        GameManager.instance.DestroyCurrentKey(wasGood, wasPressed);
    }

    private void ChangeOpacity(float i)
    {
        var tempColor = _backColor.color;
        tempColor.a = i;
        _backColor.color = tempColor;
    }
}