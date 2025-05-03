using System.Collections;
using UnityEngine;

[System.Serializable]
public class ShopList
{
    [SerializeField] public string[] itemNames;
    [SerializeField] public double buyRate;
    [SerializeField] public double sellRate;
}