using UnityEngine;
using System.Collections;
using System.Globalization;
using System.IO;

public class KeyPlayer : MonoBehaviour
{
    [SerializeField] private string[] fileNames;

    void Start()
    {
        Debug.Log("KEY PLAYER STARTED");


        string fileName = fileNames[Random.Range(0, fileNames.Length)] + ".txt";
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        StartCoroutine(LoadKeysFile(filePath));
    }

    IEnumerator LoadKeysFile(string filePath)
    {
        string result;

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            result = www.text;
        }
        else
        {
            result = System.IO.File.ReadAllText(filePath);
        }

        JsonWrapper jsonWrapper = JsonUtility.FromJson<JsonWrapper>(result);

        string time = "";

        foreach (var keyTime in jsonWrapper.keyTimes)
        {
            Debug.Log("Key: " + keyTime.Key + ", Time: " + keyTime.Time);

            time = keyTime.Time.ToString().Trim();
            time = time.Replace(",", ".");

            GameManager.instance.AddKeyEvent(
                float.Parse(time, CultureInfo.InvariantCulture), //Time
                (ArrowKey)System.Enum.Parse(typeof(ArrowKey), keyTime.Key) //Key
            );
        }

        GameManager.instance.CreateKeys();
    }
}

[System.Serializable]
public class JsonWrapper
{
    public KeyTime[] keyTimes;
}

[System.Serializable]
public class KeyTime
{
    public string Key;
    public string Time;
}