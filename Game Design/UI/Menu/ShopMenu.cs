using System.Collections;
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
    [SerializeField] public Button buyTabSelect;
    [SerializeField] public Button buyButton;
    [SerializeField] public Button sellButton;
    [SerializeField] public Button itemButtonPrefab;
    [SerializeField] public TextMeshProUGUI shopNameText;
    [SerializeField] public TextMeshProUGUI itemNameText;
    [SerializeField] public TextMeshProUGUI itemDescriptionText;
    [SerializeField] public TextMeshProUGUI itemPriceText;
    [SerializeField] public TextMeshProUGUI itemQuantityText;
    [SerializeField] public TextMeshProUGUI playerBitText;
    [SerializeField] public Transform listLayout;

    //variables to be assigned by ShopObject class
    public double provincePriceRate;
    public ShopList shopList;

    //private variables
    private string _buyOrSell = "BUY";
    private Item _item;
    private string _itemName;

    public override void Start()
    {
        base.Start();
        buyTabSelect?.Select();
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
        // SetUpButtons();
        SetUpShop();
    }

    /// <summary>
    /// Displays the item information
    /// of the item you selected. From there
    /// it will determine if you will buy 
    /// or sell the item.
    /// </summary>
    public void OnItemSelectedButton()
    {
        _item = ItemMaker.Instance.GetItemBasedOnName(_itemName);
        _itemName = item.Name;
        itemNameText.text = _item.Name;
        itemDescriptionText.text = _item.Description;
        itemPriceText.text = GetItemPrice().ToString();
        SetUpButtons();
        //TODO: enable buttons for quanity amount

    }

    public void OnPurchase()
    {
        
    }

    public void OnSell()
    {

    }

    private void SetUpButtons()
    {
        buyButton.interactable = _buyOrSell != "SELL";
        sellButton.interactable = _buyOrSell == "SELL";
    }

    private void SetUpShop()
    {
        Player player = Player.Instance();
        ClearContents();
        switch(_buyOrSell)
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

    private void SetUpBuy()
    {
        //TODO: Display list of items that are for sale at shop based on ShopList
    }

    private void SetUpSell()
    {
        //TODO: Diaplay player's items that they can sell
    }

    private void ClearContents()
    {
        Player player = Player.Instance();
        _item = null;
        _itemName = null;

        playerBitText.text = player.Bits.ToString();
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemPriceText.text = "";
        itemQuantityText.text = "_";

        foreach(Transform child in listLayout)
        {
            Destroy(child.gameObject);
        }
    }

    private int GetItemPrice()
    {
        if(_buyOrSell.Equals("SELL"))
            return (int)((double)_item.Price * shopList.sellRate);
        else
            return (int)((double)_item.Price * shopList.buyRate);
    }

}