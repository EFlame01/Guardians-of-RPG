using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InventoryMenu is a class that extends the
/// <c>MenuState</c> class. InventoryMenu
/// allows you to look at the <c>Player</c>'s entire
/// inventory divided into sections, along with
/// the ability to equip, discard, and to use said
/// items.
/// </summary>
public class InventoryMenu : MenuState
{
    //Serialized Variables
    [SerializeField] TextMeshProUGUI itemTypeText;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemDescriptionText;
    [SerializeField] Button itemButtonObjectPrefab;
    [SerializeField] Transform listLayout;
    [SerializeField] Button discardButton;
    [SerializeField] Button equipButton;
    [SerializeField] Button useButton;
    [SerializeField] DiscardMenuOption discardMenuOptionWindow;

    //private variables
    private ItemType itemType;
    private Item chosenItem;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        itemType = ItemType.FOOD;
        SetUpInventory();
    }

    // Update is called once per frame
    void Update()
    {
        CheckButtons();
    }

    /// <summary>
    /// Changes the type of items
    /// needed to display in the inventory.
    /// </summary>
    /// <param name="type"></param>
    public void OnButtonTypePressed(string type)
    {
        itemType = type switch{
            "FOOD" => ItemType.FOOD,
            "HEALING" => ItemType.HEALING,
            "KEY" => ItemType.KEY,
            "MEDICAL" => ItemType.MEDICAL,
            "PRIORITY" => ItemType.PRIORITY,
            "STAT_CHANGING" => ItemType.STAT_CHANGING,
            _ => ItemType.KEY
        };
        SetUpInventory();
    }

    /// <summary>
    /// After determining whether the item should
    /// be equipped, it equips the item to the player
    /// and displays the results.
    /// </summary>
    public void OnEquipButtonPressed()
    {
        Player player = Player.Instance();
        string result = CanEquipItem();

        if(result.Equals("You've equiped the " + chosenItem.Name + "!"))
            player.Inventory.EquipItem(chosenItem.Name);

        SetUpInventory();
        itemDescriptionText.text = result;
        
    }

    /// <summary>
    /// After determining whether the item should
    /// be discarded, it discards the items from
    /// the inventory and displays the results.
    /// </summary>
    public void OnDiscardButtonPressed()
    {
        string result = CanDiscardItem();
        if(result.Equals("I wouldn't discard this. It could be important."))
        {
            SetUpInventory();
            itemDescriptionText.text = result;
            return;
        }  

        discardMenuOptionWindow.maxAmount = Player.Instance().Inventory.ItemList[chosenItem.Name];
        discardMenuOptionWindow.gameObject.SetActive(true);
        discardMenuOptionWindow.SetUpWindow();
        discardMenuOptionWindow.confirmButton.onClick.AddListener(() =>
        {
            Player.Instance().Inventory.ChangeItemAmount(chosenItem.Name, -1 * discardMenuOptionWindow.GetItemAmount());
            StartCoroutine(discardMenuOptionWindow.ResetWindow());
            SetUpInventory();
            itemDescriptionText.text = result;
        });
    }

    /// <summary>
    /// After determining if the item can be used,
    /// It uses the item for the player and displays
    /// the results.
    /// </summary>
    public void OnUseButtonPressed()
    {
        string result = CanUseItem();
        if(result.Equals("It won't have any effect."))
        {
            SetUpInventory();
            itemDescriptionText.text = result;
        }
        else
        {
            chosenItem.UseItem(Player.Instance());
            Player.Instance().Inventory.ChangeItemAmount(chosenItem.Name, -1);
            SetUpInventory();
            itemDescriptionText.text = result;
        }
    }

    private void CheckButtons()
    {
        if(chosenItem == null)
        {
            discardButton.interactable = false;
            equipButton.interactable = false;
            useButton.interactable = false;
        }
        else if(chosenItem != null && chosenItem.Type.Equals(ItemType.KEY))
        {
            discardButton.interactable = true;
            equipButton.interactable = false;
            useButton.interactable = false;
        }
        else if(chosenItem != null && !chosenItem.Type.Equals(ItemType.KEY))
        {
            discardButton.interactable = true;
            equipButton.interactable = true;
            useButton.interactable = true;
        }
    }

    private void SetUpInventory()
    {
        Player player = Player.Instance();
        ClearContents();
        itemTypeText.text = itemType.ToString().Replace("_", " ");

        foreach(KeyValuePair<string, int> itemInfo in player.Inventory.ItemList)
        {
            Item i = ItemMaker.Instance.GetItemBasedOnName(itemInfo.Key);
            if(i != null && i.Type.Equals(itemType) && itemInfo.Value > 0)
            {
                Button button = Instantiate(itemButtonObjectPrefab, listLayout);
                TextMeshProUGUI itemNameText = button.GetComponentsInChildren<TextMeshProUGUI>()[0];
                TextMeshProUGUI itemAmountText = button.GetComponentsInChildren<TextMeshProUGUI>()[1];

                itemNameText.text = itemInfo.Key;
                itemAmountText.text = "X" + itemInfo.Value;

                button.onClick.AddListener(() =>
                {
                   SetItem(i); 
                });
            }
        }
    }

    private void SetItem(Item item)
    {
        chosenItem = item;
        itemNameText.text = item.Name;
        itemDescriptionText.text = item.Description;
    }

    private void ClearContents()
    {
        chosenItem = null;
        itemTypeText.text = "";
        itemDescriptionText.text = "";
        itemNameText.text = "";
        foreach(Transform child in listLayout)
        {
            Destroy(child.gameObject);
        }
    }

    private string CanUseItem()
    {
        Player player = Player.Instance();
        if(chosenItem == null)
            return itemDescriptionText.text;
        
        switch(itemType)
        {
            case ItemType.FOOD:
                if(player.BaseStats.Hp < player.BaseStats.FullHp)
                    return "Your health was restored.";
                else
                    return "It won't have any effect.";
            case ItemType.MEDICAL:
                MedicalItem medicalItem = (MedicalItem) chosenItem;
                if(player.BaseStats.Hp < player.BaseStats.FullHp && (medicalItem._healAmount > 0 || medicalItem._healAmount < 0))
                    return "Your health was restored.";
                return medicalItem.CanCureStatusConditions(player) ? "Your status condition was healed." : "It won't have any effect.";
            default:
                return "It won't have any effect.";
        }
    }

    private string CanEquipItem()
    {
        if(chosenItem == null)
            return itemDescriptionText.text;
        
        if(Player.Instance().Item != null)
            return "You are already holding an item. Unequip item from the Player Information section first.";
        else
            return "You've equiped the " + chosenItem.Name + "!";
    }

    private string CanDiscardItem()
    {
        if(chosenItem == null)
            return itemDescriptionText.text;
        if(chosenItem.Type.Equals(ItemType.KEY))
            return "I wouldn't discard this. It could be important.";
        else
            return "You've discarded the " + chosenItem.Name + "!";
    }
}
