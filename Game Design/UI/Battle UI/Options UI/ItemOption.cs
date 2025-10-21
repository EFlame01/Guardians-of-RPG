using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ItemOption is a class that handles
/// the UI responsible for selecting an <c>Item</c>
/// the <c>Player</c> can use or equip in
/// the next round.
/// </summary>
public class ItemOption : MonoBehaviour
{
    //serialized variables
    public Transform ItemListLayout;
    public GameObject ItemButtonPrefab;
    public TextMeshProUGUI ItemTypeText;
    public TextMeshProUGUI DescriptionText;
    public Button NextButton;

    //private variables
    private ItemType _itemType = ItemType.FOOD;
    private string _itemName;

    public void Start()
    {
        InitializeItemOption();
    }

    public void Update()
    {
        NextButton.interactable = _itemName != null;
    }

    /// <summary>
    /// Updates the list of items in the UI based
    /// on the <paramref name="itemType"/> selected.
    /// </summary>
    /// <param name="itemType">The type of <c>Item</c></param>
    public void OnItemTypeButtonPressed(string itemType)
    {
        _itemType = itemType switch
        {
            "FOOD" => ItemType.FOOD,
            "HEALING" => ItemType.HEALING,
            "MEDICAL" => ItemType.MEDICAL,
            "PRIORITY" => ItemType.PRIORITY,
            "STAT CHANGING" => ItemType.STAT_CHANGING,
            _ => ItemType.FOOD,
        };
        InitializeItemOption();
    }

    private void InitializeItemOption()
    {
        ItemTypeText.text = _itemType.ToString();
        ClearItemList();
        foreach (KeyValuePair<string, int> itemInfo in Player.Instance().Inventory.ItemList)
        {
            if (itemInfo.Value <= 0)
                continue;
            Item item = ItemMaker.Instance.GetItemBasedOnName(itemInfo.Key);
            if (item.Type.Equals(_itemType))
            {
                Button itemButton = Instantiate(ItemButtonPrefab, ItemListLayout).GetComponent<Button>();
                TextMeshProUGUI itemNameText = itemButton.GetComponentsInChildren<TextMeshProUGUI>()[0];
                TextMeshProUGUI itemAmountText = itemButton.GetComponentsInChildren<TextMeshProUGUI>()[1];

                itemNameText.text = itemInfo.Key;
                itemAmountText.text = "X" + itemInfo.Value;

                itemButton.onClick.AddListener(() =>
                {
                    _itemName = itemInfo.Key;
                    SetItemDescription();
                });
            }
        }
    }

    private void ClearItemList()
    {
        _itemName = null;
        DescriptionText.text = "";
        foreach (Transform child in ItemListLayout)
            Destroy(child.gameObject);
    }

    private void SetItemDescription()
    {
        Item item = ItemMaker.Instance.GetItemBasedOnName(_itemName);

        Player.Instance().BattleStatus.ChosenItem = item;
        DescriptionText.text = item.Description;
    }
}