using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmationSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI archetypeText;
    [SerializeField] private TextMeshProUGUI bitsText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI limText;
    [SerializeField] private TextMeshProUGUI currText;
    [SerializeField] private TextMeshProUGUI atkStatText;
    [SerializeField] private TextMeshProUGUI defStatText;
    [SerializeField] private TextMeshProUGUI evaStatText;
    [SerializeField] private TextMeshProUGUI hpStatText;
    [SerializeField] private TextMeshProUGUI spdStatText;
    [SerializeField] private Image playerImage;
    [SerializeField] private Sprite maleSprite;
    [SerializeField] private Sprite femaleSprite;

    public void OnEnable()
    {
        SetUpPlayerInformation();
        IntroScene.CurrentStory.variablesState["stateStatus"] = "";
    }


    private void SetUpPlayerInformation()
    {
        Player player = Player.Instance();
        Archetype archetype = Archetype.GetArchetype(IntroScene.ArchetypeName);

        nameText.text = IntroScene.PlayerName;
        archetypeText.text = IntroScene.ArchetypeName;
        bitsText.text = player.Bits.ToString();

        levelText.text = player.Level.ToString();
        limText.text = player.LimXP.ToString();
        currText.text = player.CurrXP.ToString();

        atkStatText.text = archetype.BaseStats.Atk.ToString();
        defStatText.text = archetype.BaseStats.Def.ToString();
        evaStatText.text = archetype.BaseStats.Eva.ToString();
        spdStatText.text = archetype.BaseStats.Spd.ToString();
        hpStatText.text = archetype.BaseStats.FullHp.ToString();

        playerImage.sprite = IntroScene.SexName.Equals("MALE") || (IntroScene.SexName.Equals("MALEFE") && GameManager.Instance.Leaning.Equals("MALE")) ? maleSprite : femaleSprite;
    }
}