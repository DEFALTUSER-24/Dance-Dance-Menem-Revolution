using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class KeyPlayer : MonoBehaviour
{
    private List<KeyValuePair<float, ArrowKey>> keyEvents = new List<KeyValuePair<float, ArrowKey>>();
    private int     currentEvent = 0;
    private float   startTime;
    public  string  fileName;

    void Start()
    {
        // Read the key events from the CSV file
        string filePath = @"C:\Users\Usuario\Documents\" + fileName + ".csv"; ;
        string[] lines = File.ReadAllLines(filePath);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(';');
            ArrowKey key = (ArrowKey)System.Enum.Parse(typeof(ArrowKey), parts[0]);
            float time = float.Parse(parts[1]) - GameManager.instance.delayTime;
            keyEvents.Add(new KeyValuePair<float, ArrowKey>(time, key));
        }

        // Start the timer
        startTime = Time.time;
    }

    void Update()
    {
        // Check if it's time to trigger the next key event
        if (currentEvent < keyEvents.Count)
        {
            float elapsedTime = Time.time - startTime;
            GameManager.instance.timer = elapsedTime;
            float eventTime = keyEvents[currentEvent].Key;
            if (elapsedTime >= eventTime)
            {
                ArrowKey key = keyEvents[currentEvent].Value;
                GameManager.instance.ShowArrow(key, eventTime);
                Debug.Log("Key: " + key + " pressed at: " + elapsedTime);
                currentEvent++;
            }
        }
    }
}