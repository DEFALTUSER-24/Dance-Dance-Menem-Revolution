using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class KeyPlayer : MonoBehaviour
{
    private List<KeyValuePair<float, ArrowKey>> keyEvents = new List<KeyValuePair<float, ArrowKey>>();
    public  string  fileName;

    void Start()
    {
        // Read the key events from the CSV file
        string filePath = Path.Combine(Application.dataPath, fileName + ".csv");
        string[] lines = File.ReadAllLines(filePath);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(';');
            ArrowKey key = (ArrowKey)System.Enum.Parse(typeof(ArrowKey), parts[0]);
            float time = float.Parse(parts[1]) - GameManager.instance.delayTime;
            keyEvents.Add(new KeyValuePair<float, ArrowKey>(time, key));
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(';');
            ArrowKey key = (ArrowKey)System.Enum.Parse(typeof(ArrowKey), parts[0]);
            float time = float.Parse(parts[1]);
            GameManager.instance.AddKeyEvent(time, key);
        }
    }
}