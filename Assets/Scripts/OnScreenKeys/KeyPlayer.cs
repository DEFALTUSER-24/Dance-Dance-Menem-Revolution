using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class KeyPlayer : MonoBehaviour
{
    [SerializeField] private string[] fileNames;

    void Start()
    {
        Debug.Log("KEY PLAYER STARTED");

        string filePath = Path.Combine(Application.streamingAssetsPath, fileNames[Random.Range(0, fileNames.Length)] + ".csv");
        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist.");
            return;
        }

        // Read the CSV file and skip the first row
        string[] lines = File.ReadAllLines(filePath);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(';');

            GameManager.instance.AddKeyEvent(
                float.Parse(fields[1]), //Time
                (ArrowKey)System.Enum.Parse(typeof(ArrowKey), fields[0]) //Key
            );
        }

        GameManager.instance.CreateKeys();
    }
}