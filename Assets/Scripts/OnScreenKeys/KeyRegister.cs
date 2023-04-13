using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class KeyRegister : MonoBehaviour
{
    [SerializeField] private  string  fileName;

    private Dictionary<ArrowKey, float> keyDownTimes = new Dictionary<ArrowKey, float>();
    private string filePath;

    private HashSet<KeyCode> validKeyCodes;
    private Dictionary<KeyCode, ArrowKey> arrowKeyToKeyCodeMap;

    void Start()
    {
        Debug.Log("KEY REGISTER STARTED");

        validKeyCodes = new HashSet<KeyCode> { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
        arrowKeyToKeyCodeMap = new Dictionary<KeyCode, ArrowKey> {
            { KeyCode.UpArrow, ArrowKey.Up },
            { KeyCode.DownArrow, ArrowKey.Down },
            { KeyCode.LeftArrow, ArrowKey.Left },
            { KeyCode.RightArrow, ArrowKey.Right }
        };

        //If StreamingAssets directory doesn't exists.
        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);

        filePath = Path.Combine(Application.streamingAssetsPath, fileName + ".csv");

        //If file doesn't exists.
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "Key;Time\n");
        else
            Debug.LogWarning("File already exists. Trying to overwrite?");
    }

    void Update()
    {
        foreach (KeyCode kcode in validKeyCodes)
        {
            if (Input.GetKeyDown(kcode))
            {
                LogKey(arrowKeyToKeyCodeMap[kcode]);
                break;
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