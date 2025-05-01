using System.Collections;
using UnityEngine;

[System.Serializable]
public class ShopList
{
    [SerializeField] string[] itemNames;
    [SerializeField] double buyRate;
    [SerializeField] double sellRate;
}