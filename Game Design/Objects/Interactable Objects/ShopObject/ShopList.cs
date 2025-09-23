using System.Collections;
using UnityEngine;

/// <summary>
/// ShopList is a class that holds the
/// list of shop items and the buy and 
/// sell rate.
/// </summary>
[System.Serializable]
public class ShopList
{
    //Serialized variables
    [SerializeField] public string[] itemNames;
    [SerializeField] public double buyRate;
    [SerializeField] public double sellRate;
}