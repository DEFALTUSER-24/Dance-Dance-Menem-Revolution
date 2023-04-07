using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class KeyRegister : MonoBehaviour
{
    private Dictionary<ArrowKey, float> keyDownTimes = new Dictionary<ArrowKey, float>();
    public  string  fileName;
    private string  filePath;

    void Start()
    {
        filePath = Path.Combine(Application.dataPath, fileName + ".csv");
        // Create the file if it doesn't exist
        
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "Key;Time\n");
    }

    void Update()
    {
        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                ArrowKey? arrowKey = null;
                switch (kcode)
                {
                    case KeyCode.UpArrow:
                        arrowKey = ArrowKey.Up;
                        break;
                    case KeyCode.DownArrow:
                        arrowKey = ArrowKey.Down;
                        break;
                    case KeyCode.LeftArrow:
                        arrowKey = ArrowKey.Left;
                        break;
                    case KeyCode.RightArrow:
                        arrowKey = ArrowKey.Right;
                        break;
                }
                if (arrowKey.HasValue)
                {
                    LogKey(arrowKey.Value);
                }
            }
        }
    }

    private void LogKey(ArrowKey key)
    {
        keyDownTimes[key] = Time.time;
        Debug.Log("Key: " + key + " pressed at: " + Time.time);
        File.AppendAllText(filePath, key + ";" + Time.time + "\n");
    }
}