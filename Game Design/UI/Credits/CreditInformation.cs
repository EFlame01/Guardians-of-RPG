using System;
using UnityEngine;

[Serializable]
public class CreditInformation
{
    [SerializeField] public CreditType Type;
    [SerializeField] public string Title;
    [SerializeField] public string[] Credit;
}