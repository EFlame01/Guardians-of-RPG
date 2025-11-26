using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoleCredit : Credit
{
    [SerializeField] public TextMeshProUGUI RoleText;
    [SerializeField] public TextMeshProUGUI NameText;

    public override void UpdateText(CreditInformation creditInformation)
    {
        RoleText.text = creditInformation.Title;
        NameText.text = "";
        foreach(string credit in creditInformation.Credit)
            NameText.text += credit + "\n";
    }
}