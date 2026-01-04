using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class PlayerMenu : MenuState
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerArchetypeText;
    [SerializeField] private TextMeshProUGUI playerSexText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private SliderBar playerXpSlider;
    [SerializeField] private TextMeshProUGUI playerCurrentXPText;
    [SerializeField] private TextMeshProUGUI playerLimitXPText;
    [SerializeField] private TextMeshProUGUI playerBitsText;
    [SerializeField] private SliderBar playerHpSlider;
    [SerializeField] private TextMeshProUGUI playerHpText;
    [SerializeField] private TextMeshProUGUI playerFullHpText;
    [SerializeField] private TextMeshProUGUI playerAtkText;
    [SerializeField] private TextMeshProUGUI playerDefText;
    [SerializeField] private TextMeshProUGUI playerEvaText;
    [SerializeField] private TextMeshProUGUI playerSpdText;
    [SerializeField] private TextMeshProUGUI playerAbilityText;
    [SerializeField] private TextMeshProUGUI playerItemText;
    [SerializeField] private Button unEquipButton;
    [SerializeField] private Image playerImage;
    [SerializeField] private Sprite maleSprite;
    [SerializeField] private Sprite femaleSprite;

    public override void Start()
    {
        base.Start();
        SetUpPlayerInformation();
    }

    public void SetUpPlayerInformation()
    {
        Player player = Player.Instance();

        playerNameText.text = player.Name;
        playerAbilityText.text = player.Ability == null ? "Nothing" : player.Ability.Name;
        playerArchetypeText.text = player.Archetype.ArchetypeName;
        playerAtkText.text = player.BaseStats.Atk.ToString();
        playerBitsText.text = player.Bits.ToString();
        playerCurrentXPText.text = player.CurrXP.ToString();
        playerDefText.text = player.BaseStats.Def.ToString();
        playerEvaText.text = player.BaseStats.Eva.ToString();
        playerFullHpText.text = player.BaseStats.FullHp.ToString();
        playerHpText.text = player.BaseStats.Hp.ToString();
        playerItemText.text = player.Item == null ? "Nothing" : player.Item.Name;
        playerLevelText.text = player.Level.ToString();
        playerLimitXPText.text = player.LimXP.ToString();
        playerSexText.text = player.Sex;
        playerSpdText.text = player.BaseStats.Spd.ToString();

        string sex = player.Sex.Equals("MALEFE") ? GameManager.Instance.Leaning : player.Sex;
        playerImage.sprite = sex.Equals("FEMALE") ? femaleSprite : maleSprite;

        playerHpSlider.SetValue(player.BaseStats.Hp, player.BaseStats.FullHp);
        playerXpSlider.SetValue(player.CurrXP, player.LimXP);

        unEquipButton.interactable = player.Item != null;
    }

    public void OnUnEquipButtonPressed()
    {
        Player player = Player.Instance();
        string itemName = player.Item.Name;
        string itemType = player.Item.Type.ToString();
        player.UnequipItemFromPlayer();
        playerItemText.text = player.Item == null ? "Nothing" : player.Item.Name;
        unEquipButton.interactable = player.Item != null;
        //TODO: narrate that item has been unequipped from player
    }
}
