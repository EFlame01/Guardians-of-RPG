using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapLocation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI locationText;
    [SerializeField] Animator animator;

    private static string currentMapLocation  = "Tiro Town";

    public string[] majorLocations = 
    {
        "Tiro Town",
        "Argus Beach",
        "Toxic Trails",
        "Esoic City",
        "Lonaturus",
        "Asale Dune",
        "Asale",
        "Primal City",
        "Berserker Tribe",
        "Terminal Beach",
        "Beta Beach",
        "Gormania A.",
        "Promiterra",
        "No Man's Land",
        "Promiterra"
    };

    // Start is called before the first frame update
    void Start()
    {
        //TODO: check if player is in a new environment
        CheckForMajorLocation();
    }

    public static string GetCurrentMapLocation()
    {
        return currentMapLocation;
    }

    public static void SetCurrentMapLocation(string locationName)
    {
        currentMapLocation = locationName;
    }

    private void CheckForMajorLocation()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        foreach(string location in majorLocations)
        {
            if(location.Contains(currentSceneName))
            {
                if(!currentMapLocation.Equals(location))
                {
                    currentMapLocation = location;
                    locationText.text = location;
                    animator.Play("open");
                }
                break;
            }
        }
    }
}
