using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArchetypeSelection : MonoBehaviour
{
    [SerializeField] private Button archetype1;
    [SerializeField] private Button archetype2;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI evaText;
    [SerializeField] private TextMeshProUGUI spdText;
    [SerializeField] private TextMeshProUGUI hpText;

    public void OnEnable()
    {
        ClearText();
        InitArchetypeButtons();
        IntroScene.ArchetypeName = "";
        IntroScene.CurrentStory.variablesState["stateStatus"] = "";
    }

    public void OnArchetypeButtonPressed(int archetypeOption)
    {
        string archetype1Name = archetype1.GetComponentInChildren<TextMeshProUGUI>().text;
        string archetype2Name = archetype2.GetComponentInChildren<TextMeshProUGUI>().text;
        string archetypeName = archetypeOption == 1 ? archetype1Name : archetype2Name;
        BaseStats archetypeBaseStats = Archetype.GetArchetype(archetypeName).BaseStats;

        atkText.text = archetypeBaseStats.Atk.ToString();
        defText.text = archetypeBaseStats.Def.ToString();
        evaText.text = archetypeBaseStats.Eva.ToString();
        spdText.text = archetypeBaseStats.Spd.ToString();
        hpText.text = archetypeBaseStats.Hp.ToString();

        IntroScene.ArchetypeName = archetypeName;
        IntroScene.CurrentStory.variablesState["stateStatus"] = "next";
        Debug.Log("Archetype selected...");
    }

    private void ClearText()
    {
        atkText.text = "00";
        defText.text = "00";
        evaText.text = "00";
        spdText.text = "00";
        hpText.text = "00";
    }

    private void InitArchetypeButtons()
    {
        switch (IntroScene.ClassName)
        {
            case "SWORDSMAN":
                archetype1.GetComponentInChildren<TextMeshProUGUI>().text = "REGULAR SWORDSMAN";
                archetype2.GetComponentInChildren<TextMeshProUGUI>().text = "DUAL SWORDSMAN";
                break;
            case "DEFENDER":
                archetype1.GetComponentInChildren<TextMeshProUGUI>().text = "KNIGHT";
                archetype2.GetComponentInChildren<TextMeshProUGUI>().text = "HEAVY SHIELDER";
                break;
            case "ESOIC":
                archetype1.GetComponentInChildren<TextMeshProUGUI>().text = "ENERGY MANIPULATOR";
                archetype2.GetComponentInChildren<TextMeshProUGUI>().text = "NATURE MANIPULATOR";
                break;
            case "BRAWLER":
                archetype1.GetComponentInChildren<TextMeshProUGUI>().text = "MIXED MARTIAL ARTIST";
                archetype2.GetComponentInChildren<TextMeshProUGUI>().text = "BERSERKER";
                break;
            case "SPECIALIST":
                archetype1.GetComponentInChildren<TextMeshProUGUI>().text = "COMBAT SPECIALIST";
                archetype2.GetComponentInChildren<TextMeshProUGUI>().text = "WEAPON SPECIALIST";
                break;
        }
    }
}