using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MenuState
{
    [SerializeField] public Button buyTabSelect;
    [SerializeField] public Button buyButton;
    [SerializeField] public Button sellButton;
    [SerializeField] public TextMeshProUGUI itemNameText;
    [SerializeField] public TextMeshProUGUI itemDescriptionText;
    [SerializeField] public TextMeshProUGUI itemPriceText;
    [SerializeField] public TextMeshProUGUI itemQuantityText;
    [SerializeField] public TextMeshProUGUI playerBitText;
    [SerializeField] public Transform listLayout;
    [SerializeField] public double provincePriceRate;
    [SerializeField] public TextMeshProUGUI shopNameText;
    // [SerializeField] public ShopList shopList;

    private string _buyOrSell = "BUY";
    private Item _item;
    private string _itemName;

    public override void Start()
    {
        base.Start();
        buyTabSelect?.Select();
        OnCommerceTabSelected(_buyOrSell);
    }

    public void OnCommerceTabSelected(string buyOrSell)
    {
        _buyOrSell = buyOrSell switch
        {
            "BUY" => "BUY",
            "SELL" => "SELL",
            _ => "BUY"
        };
        SetUpButtons();
        SetUpShop();
    }

    public void OnItemSelectedButton()
    {
        _item = ItemMaker.Instance.GetItemBasedOnName(_itemName);
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

}