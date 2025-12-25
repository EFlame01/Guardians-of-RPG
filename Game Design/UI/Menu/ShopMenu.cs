using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ShopMenu is a class that extends the <c>MenuState</c>
/// class. ShopMenu will be activated once the 
/// <c>ShopObject</c> has been interacted with. From
/// there, the ShopMenu will open a mneu that allows you
/// to buy and sell things from a shop, with prices
/// varying based on the price rate.
/// </summary>
public class ShopMenu : MenuState
{
    //Serialize variables
    public Button buyTabSelect;
    public Button buyButton;
    public Button sellButton;
    public Button lessButton;
    public Button moreButton;
    public Button itemButtonPrefab;
    public Button itemButtonPrefab2;
    public TextMeshProUGUI shopNameText;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemPriceText;
    public TextMeshProUGUI itemQuantityText;
    public TextMeshProUGUI playerBitText;
    public GameObject buyLayout;
    public GameObject sellLayout;
    public Transform buyListLayout;
    public Transform sellListLayout;

    //variables to be assigned by ShopObject class
    public ShopList shopList;
    public string shopName;
    public ItemType itemType = ItemType.FOOD;

    //private variables
    private string _buyOrSell = "BUY";
    private Item _item;
    private int _itemAmount;

    public override void Start()
    {
        base.Start();
        buyTabSelect?.Select();
        buyButton.interactable = false;
        sellButton.interactable = false;
        OnCommerceTabSelected(_buyOrSell);
    }

    /// <summary>
    /// Changes the setting in the ShopMenu
    /// from either buying or selling.
    /// </summary>
    /// <param name="buyOrSell"></param>
    public void OnCommerceTabSelected(string buyOrSell)
    {
        _buyOrSell = buyOrSell switch
        {
            "BUY" => "BUY",
            "SELL" => "SELL",
            _ => "BUY"
        };
        SetUpShop();
    }

    /// <summary>
    /// Displays the item information
    /// of the item you selected. From there
    /// it will determine if you will buy 
    /// or sell the item.
    /// </summary>
    public void OnItemSelectedButton(Item item)
    {
        _item = item;
        _itemAmount = 1;
        itemQuantityText.text = _itemAmount.ToString();
        itemNameText.text = _item.Name;
        itemDescriptionText.text = _item.Description;
        itemPriceText.text = GetItemPrice().ToString();
        SetUpButtons();
        lessButton.interactable = true;
        moreButton.interactable = true;
    }

    /// <summary>
    /// Purchases the item from the shop and adds the items
    /// to the player's inventory.
    /// </summary>
    public void OnPurchase()
    {
        Player player = Player.Instance();
        int price = GetItemPrice();
        int level = _item.Level;
        string itemName = _itemAmount > 1 ? _item.PluralName : _item.Name;

        if (level > player.Level)
        {
            SetUpShop();
            itemDescriptionText.text = "You are not a high enough level to obtain this.";
            return;
        }
        else if (price > Player.Instance().Bits)
        {
            SetUpShop();
            itemDescriptionText.text = "You do not have enough bits to obtain this.";
            return;
        }

        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.PURCHASED);
        player.Inventory.AddItem(_item.Name, _itemAmount);
        player.SetBits(player.Bits - price);
        SetUpShop();
        itemDescriptionText.text = "You purchased " + _itemAmount + " " + itemName + "!";
    }

    /// <summary>
    /// Sells the player's items to the shop and 
    /// removes the items from the player's inventory
    /// </summary>
    public void OnSell()
    {
        Player player = Player.Instance();
        int price = GetItemPrice();
        string itemName = _itemAmount > 1 ? _item.PluralName : _item.Name;

        if (_itemAmount > player.Inventory.ItemList[_item.Name])
        {
            SetUpShop();
            itemDescriptionText.text = "You do not have enough to sell.";
            return;
        }

        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.PURCHASED);
        player.Inventory.AddItem(_item.Name, -1 * _itemAmount);
        player.SetBits(player.Bits + price);
        SetUpShop();
        itemDescriptionText.text = "You sold " + _itemAmount + " " + itemName + "!";
    }

    /// <summary>
    /// Decrements the number of items the player
    /// may wish to purchase/sell.
    /// </summary>
    public void OnLessButtonPressed()
    {
        _itemAmount = Mathf.Clamp(_itemAmount - 1, 0, _itemAmount);
        itemQuantityText.text = _itemAmount.ToString();
        itemPriceText.text = GetItemPrice().ToString();
    }

    /// <summary>
    /// Increments the number of items the player
    /// may wish to purchase/sell.
    /// </summary>
    public void OnMoreButtonPressed()
    {
        _itemAmount = Mathf.Clamp(_itemAmount + 1, 1, 100);
        itemQuantityText.text = _itemAmount.ToString();
        itemPriceText.text = GetItemPrice().ToString();
    }

    /// <summary>
    /// Changes the list of items being displayed
    /// in the player's inventory
    /// </summary>
    /// <param name="itemTypeOption"></param>
    public void OnItemTypePressed(string itemTypeOption)
    {
        itemType = itemTypeOption switch
        {
            "FOOD" => ItemType.FOOD,
            "HEALING" => ItemType.HEALING,
            "MEDICAL" => ItemType.MEDICAL,
            "PRIORITY" => ItemType.PRIORITY,
            "STAT_CHANGING" => ItemType.STAT_CHANGING,
            _ => ItemType.FOOD
        };

        SetUpShop();
    }

    /// <summary>
    /// Sets up the buy or sell buttons based on
    /// if the player is trying to buy or sell 
    /// something.
    /// </summary>
    private void SetUpButtons()
    {
        buyButton.interactable = _buyOrSell.Equals("BUY");
        sellButton.interactable = _buyOrSell.Equals("SELL");
        lessButton.interactable = false;
        moreButton.interactable = false;
    }

    /// <summary>
    /// Based on if the player wants to buy
    /// or sell something, this method sets
    /// up the shop to be ready for either
    /// scenario.
    /// </summary>
    private void SetUpShop()
    {
        ClearContents();
        switch (_buyOrSell)
        {
            case "BUY":
                SetUpBuy();
                break;
            case "SELL":
                SetUpSell();
                break;
            default:
                _buyOrSell = "BUY";
                SetUpButtons();
                SetUpBuy();
                break;
        }
    }

    /// <summary>
    /// Sets up the shop for the player
    /// to buy specific items unique to the
    /// store/prices.
    /// </summary>
    private void SetUpBuy()
    {
        sellLayout.SetActive(false);
        buyLayout.SetActive(true);
        foreach (string itemName in shopList.itemNames)
        {
            Item item = ItemMaker.Instance.GetItemBasedOnName(itemName);
            Button button = Instantiate(itemButtonPrefab, buyListLayout);
            TextMeshProUGUI itemNameText = button.GetComponentsInChildren<TextMeshProUGUI>()[0];
            TextMeshProUGUI itemAmountText = button.GetComponentsInChildren<TextMeshProUGUI>()[1];

            itemNameText.text = item.Name;
            itemAmountText.text = "";

            button.onClick.AddListener(() => OnItemSelectedButton(item));
        }
    }

    /// <summary>
    /// Sets up the shop for player
    /// to sell specific items in their
    /// inventory.
    /// </summary>
    private void SetUpSell()
    {
        Player player = Player.Instance();
        sellLayout.SetActive(true);
        buyLayout.SetActive(false);
        foreach (KeyValuePair<string, int> itemInfo in player.Inventory.ItemList)
        {
            Item item = ItemMaker.Instance.GetItemBasedOnName(itemInfo.Key);
            if (item != null && item.Type.Equals(itemType) && itemInfo.Value > 0)
            {
                Button button = Instantiate(itemButtonPrefab2, sellListLayout);
                TextMeshProUGUI itemNameText = button.GetComponentsInChildren<TextMeshProUGUI>()[0];
                TextMeshProUGUI itemAmountText = button.GetComponentsInChildren<TextMeshProUGUI>()[1];

                itemNameText.text = itemInfo.Key;
                itemAmountText.text = "X" + itemInfo.Value;

                button.onClick.AddListener(() => OnItemSelectedButton(item));
            }
        }
    }

    /// <summary>
    /// Clears the list of items in the 
    /// buyListLayout or sellListLayout.
    /// </summary>
    private void ClearContents()
    {
        Player player = Player.Instance();
        _item = null;

        playerBitText.text = player.Bits.ToString();
        shopNameText.text = shopName;
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemPriceText.text = "0";
        itemQuantityText.text = "_";

        buyButton.interactable = false;
        sellButton.interactable = false;
        lessButton.interactable = false;
        moreButton.interactable = false;

        if (_buyOrSell.Equals("SELL"))
        {
            foreach (Transform child in sellListLayout)
                Destroy(child.gameObject);
        }
        else
        {
            foreach (Transform child in buyListLayout)
                Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Uses the sellRate/buyRate of the 
    /// store to calculate the price of 
    /// an item to be bought/sold. It 
    /// returns the amount of bits in 
    /// the form of an int.
    /// </summary>
    /// <returns>The item price to be bought/sold</returns>
    private int GetItemPrice()
    {
        if (_buyOrSell.Equals("SELL"))
            return (int)(_item.Price * shopList.sellRate * _itemAmount);
        else
            return (int)(_item.Price * shopList.buyRate * _itemAmount);
    }

}