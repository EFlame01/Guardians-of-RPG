using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DataRetriever : Singleton<DataRetriever>
{
    private string Data;
    public string[] Database { get; private set; }
    public static bool initializedStreamingAssets;

    protected override void Awake()
    {
        base.Awake();
        if (!initializedStreamingAssets)
            StartCoroutine(InitializeStreamingAssetsData());
    }

    public void ClearData()
    {
        Data = null;
    }

    public IEnumerator InitializeStreamingAssetsData()
    {
        initializedStreamingAssets = true;
        List<string> database = new();
        foreach (string path in Units.DATABASE_PATHS)
        {
            yield return GetStreamingAssetsFileWebGL(path);
            database.Add(Data);
            ClearData();
        }

        Database = database.ToArray();
    }

    public void GetStreamingAssetsFile(string path)
    {
        try
        {
            if (File.Exists(Application.streamingAssetsPath + path))
                Data = File.ReadAllText(Application.streamingAssetsPath + path);
            else
                Debug.LogWarning("file: " + Application.streamingAssetsPath + path + " does not exist!");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            Data = null;
        }
    }

    public IEnumerator GetStreamingAssetsFileWebGL(string path)
    {
        if (Application.streamingAssetsPath.Contains("://"))
        {
            UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + path);

            yield return www.SendWebRequest();

            switch (www.result)
            {
                case UnityWebRequest.Result.Success:
                    Data = www.downloadHandler.text;
                    Debug.Log("Retrieved data for " + path);
                    break;
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("ERROR: " + www.error);
                    Data = null;
                    break;
            }
        }
        else
        {
            GetStreamingAssetsFile(path);
            yield return null;
        }
    }

    public string GetDataBasedOnID(string data, string id)
    {
        foreach (string row in data.Split('\n'))
        {
            if (row.Contains(id + ","))
                return row;
        }

        return null;
    }

    public string[] SplitDataBasedOnID(string data, string id)
    {
        List<string> rows = new();

        foreach (string row in data.Split('\n'))
        {
            if (row.Contains(id + ","))
                rows.Add(row);
        }

        return rows.ToArray();
    }

    public string[] SplitDataBasedOnRow(string data)
    {
        List<string> rows = new();
        int colNameRow = 0;

        foreach (string row in data.Split('\n'))
        {
            if (colNameRow++ == 0)
                continue;
            else
                rows.Add(row);
        }
        return rows.ToArray();
    }
}