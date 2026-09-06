
using UnityEngine;

/// <summary>
/// MapDescMaker is a class that parses through
/// the data to create <c>LocationInformation</c> struct for
/// the <c>Map</c> UI class.
/// </summary>
public class MapDescMaker : Singleton<MapDescMaker>
{
    private const int MAP_INDEX = 7;
    private const int MAP_DATA_LENGTH = 7;

    /// <summary>
    /// Gets and returns an locationInformation struct based on the <paramref name="id"/>.
    /// </summary>
    /// <param name="id">ID of the location on the map</param>
    /// <returns>the <c>LocationInformation</c> struct or <c>null</c> if an ability could not be found.</returns>
    public LocationInformation GetLocationInformationBasedOnID(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
        {
            Debug.LogWarning($"WARNING: The location information for [ {id} ] is null or empty");
            return null;
        }

        string mapData = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[MAP_INDEX], id);
        if (string.IsNullOrEmpty(mapData) || mapData.Split(',').Length != MAP_DATA_LENGTH)
        {
            Debug.LogWarning($"WARNING: An error occured looking for the location information {id}. Please report bug.");
            return null;
        }

        string[] mapAttributes = mapData.Split(',');

        return new LocationInformation
        (
            mapAttributes[1],
            float.TryParse(mapAttributes[2], out float x) ? x : 0.0f,
            float.TryParse(mapAttributes[3], out float y) ? y : 0.0f,
            mapAttributes[4],
            mapAttributes[5],
            mapAttributes[6]
        );
    }
}