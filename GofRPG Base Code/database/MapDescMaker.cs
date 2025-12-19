using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro.Examples;

/// <summary>
/// MapDescMaker is a class that parses through
/// the data to create <c>LocationInformation</c> struct for
/// the <c>Map</c> UI class.
/// </summary>
public class MapDescMaker : Singleton<MapDescMaker>
{
    private readonly string _mapDataPath = "/database/map.csv";
    private string _data;

    /// <summary>
    /// Gets and returns an locationInformation struct based on the <paramref name="id"/>.
    /// </summary>
    /// <param name="id">ID of the location on the map</param>
    /// <returns>the <c>LocationInformation</c> struct or <c>null</c> if an ability could not be found.</returns>
    public LocationInformation GetLocationInformationBasedOnID(string id)
    {
        if (id == null)
            return null;

        string[] mainAttributes;

        // DataEncoder.Instance.DecodePersistentDataFile(_mapDataPath);
        DataEncoder.Instance.GetStreamingAssetsFile(_mapDataPath);
        mainAttributes = DataEncoder.Instance.GetRowOfData(name).Split(',');
        DataEncoder.ClearData();

        return new LocationInformation
        (
            mainAttributes[1],
            float.Parse(mainAttributes[2]),
            float.Parse(mainAttributes[3]),
            mainAttributes[4],
            mainAttributes[5],
            mainAttributes[6]
        );
    }

    public LocationInformation GetLocationInformationTEST(string id)
    {
        if (id == null)
            return null;

        _data = System.IO.File.ReadAllText(Application.streamingAssetsPath + _mapDataPath);

        string[] mainAttributes = GetRowOfData(id).Split(',');

        return new LocationInformation
        (
            mainAttributes[1],
            float.Parse(mainAttributes[2]),
            float.Parse(mainAttributes[3]),
            mainAttributes[4],
            mainAttributes[5],
            mainAttributes[6]
        );
    }

    // IEnumerator Example(string filePath) {
    //     if (filePath.Contains("://")) {
    //         UnityWebRequest www = new UnityWebRequest(filePath);
    //         yield return www;
    //         _data = www.result;
    //     } else
    //         _data = System.IO.File.ReadAllText(filePath);
    // }

    public string GetRowOfData(string id)
    {
        foreach (string row in _data.Split('\n'))
        {
            if (row.Contains(id))
                return row;
        }
        return null;
    }
}