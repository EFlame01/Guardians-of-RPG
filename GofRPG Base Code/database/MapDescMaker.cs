
/// <summary>
/// MapDescMaker is a class that parses through
/// the data to create <c>LocationInformation</c> struct for
/// the <c>Map</c> UI class.
/// </summary>
public class MapDescMaker : Singleton<MapDescMaker>
{
    private const int MAP_INDEX = 7;

    /// <summary>
    /// Gets and returns an locationInformation struct based on the <paramref name="id"/>.
    /// </summary>
    /// <param name="id">ID of the location on the map</param>
    /// <returns>the <c>LocationInformation</c> struct or <c>null</c> if an ability could not be found.</returns>
    public LocationInformation GetLocationInformationBasedOnID(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        string[] mapAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[MAP_INDEX], id).Split(',');

        if (mapAttributes == null)
            return null;

        return new LocationInformation
        (
            mapAttributes[1],
            float.Parse(mapAttributes[2]),
            float.Parse(mapAttributes[3]),
            mapAttributes[4],
            mapAttributes[5],
            mapAttributes[6]
        );
    }
}