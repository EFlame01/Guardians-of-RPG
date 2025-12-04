using UnityEngine;
using TMPro;

public class ClassSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI classDescription;

    public void OnEnable()
    {
        classDescription.text = "";
        IntroScene.ClassName = "";
        IntroScene.CurrentStory.variablesState["stateStatus"] = "";
    }

    public void OnClassSelected(string className)
    {
        switch (className)
        {
            case "SWORDSMAN":
                classDescription.text = "<b>SWORDSMEN</b> typically channel BINARY energy into their blades. They are known for their strong ATTACK stats.";
                break;
            case "DEFENDER":
                classDescription.text = "<b>DEFENDERS</b> typically channel BINARY energy into their shield and armor. They are known for their strong DEFENSE stats.";
                break;
            case "ESOIC":
                classDescription.text = "<b>ESOICS</b> typically channel BINARY energy into their minds. They are known for having great HEALTH stats.";
                break;
            case "BRAWLER":
                classDescription.text = "<b>BRAWLERS</b> typically channel BINARY energy into their bodies. They are known for having great SPEED stats.";
                break;
            case "SPECIALIST":
                classDescription.text = "<b>SPECIALISTS</b> typically channel BINARY energy to the world around them. They are known for their sharp EVASION stats.";
                break;
        }

        IntroScene.ClassName = className;
        IntroScene.CurrentStory.variablesState["class"] = className;
        IntroScene.CurrentStory.variablesState["stateStatus"] = "next";
        Debug.Log("Class selected...");
    }
}