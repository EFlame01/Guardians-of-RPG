using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MenuState
{
    [SerializeField] TextMeshProUGUI itemTypeText;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemDescriptionText;
    [SerializeField] Button itemButtonObjectPrefab;
    [SerializeField] Transform listLayout;
    [SerializeField] Button discardButton;
    [SerializeField] Button equipButton;
    [SerializeField] Button useButton;
    [SerializeField] DiscardMenuOption discardMenuOptionWindow;

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

    public void OnButtonTypePressed(ItemType type)
    {
        itemType = type;
        SetUpInventory();
    }

    public void OnEquipButtonPressed()
    {
        Player player = Player.Instance();
        
        player.Inventory.EquipItem(chosenItem.Name);
        //TODO: let player know they have equipped item
        SetUpInventory();
    }

    public void OnDiscardButtonPressed()
    {
        discardMenuOptionWindow.maxAmount = Player.Instance().Inventory.ItemList[chosenItem.Name];
        discardMenuOptionWindow.gameObject.SetActive(true);
        discardMenuOptionWindow.SetUpWindow();
        discardMenuOptionWindow.confirmButton.onClick.AddListener(() =>
        {
            Player.Instance().Inventory.ChangeItemAmount(chosenItem.Name, -1 * discardMenuOptionWindow.GetItemAmount());
            StartCoroutine(discardMenuOptionWindow.ResetWindow());
            SetUpInventory();
            //TODO: Narrate item was discarded.
        });
    }

    public void OnUseButtonPressed()
    {
        string result = CanUseItem();
        //TODO: if they cannot use item, narrate that they cannot use item
        if(result.Equals("It won't have any effect."))
        {
            //Narrate why player cannot use item
            ClearContents();
            itemDescriptionText.text = result;
            SetUpInventory();
        }
        else
        {
            chosenItem.UseItem(Player.Instance());
            Player.Instance().Inventory.ChangeItemAmount(chosenItem.Name, -1);
            //TODO: narrate effects of item used
            ClearContents();
            itemDescriptionText.text = result;
            SetUpInventory();
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
        itemTypeText.text = itemType.ToString();

        foreach(KeyValuePair<string, int> itemInfo in player.Inventory.ItemList)
        {
            Item i = ItemMaker.Instance.GetItemBasedOnName(itemInfo.Key);
            if(i.Type.Equals(itemType) && itemInfo.Value > 0)
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
}
