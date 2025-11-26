using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleCredit : Credit
{
    [SerializeField] public TextMeshProUGUI TitleText;

    public override void UpdateText(CreditInformation creditInformation)
    {
        TitleText.text = creditInformation.Title;
        switch(creditInformation.Type)
        {
            case CreditType.TITLE:
                TitleText.fontSize = TITLE_SIZE;
                break;
            case CreditType.SUBTITLE: 
                TitleText.fontSize = SUBTITLE_SIZE;
                break;
            default:
                TitleText.fontSize = TITLE_SIZE;
                break;
        }
    }
}