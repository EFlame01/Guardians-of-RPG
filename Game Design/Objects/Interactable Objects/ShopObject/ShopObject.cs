using System.Collections;
using UnityEngine;

/// <summary>
/// ShopObject is a class that extends
/// the <c>InteractableObject</c> class. ShopObject
/// allow the player to buy and sell items. 
/// </summary>
public class ShopObject : InteractableObject
{
    //Serialized variables
    [SerializeField] private PlayerDirection _directionToUseShop;
    [SerializeField] private string shopName;
    [SerializeField] private ShopList shopList;
    [SerializeField] private ShopMenu shopMenu;

    //private variable
    private bool _useShop;

    /// <summary>
    /// If player can interact, calls the
    /// OpenShop() method.
    /// </summary>
    public override void InteractWithObject()
    {
        if(CanInteract && !_useShop)
        {
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            _useShop = true;
            OpenShop();
        }
    }

    /// <summary>
    /// Opens the shop menu and sets
    /// the PlayerState to PAUSED.
    /// </summary>
    private void OpenShop()
    {
        shopMenu.shopList = shopList;
        shopMenu.shopName = shopName;
        Instantiate(shopMenu, null);
        GameManager.Instance.PlayerState = PlayerState.PAUSED;
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(ObjectDetected)
            return;

        if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseShop) && GetObjectFacingSide().Equals(_directionToUseShop))
            RevealObjectIsInteractable(true);
    }
    
    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if(!ObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseShop) && GetObjectFacingSide().Equals(_directionToUseShop))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if(IsThisObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseShop) && GetObjectFacingSide().Equals(_directionToUseShop))
                RevealObjectIsInteractable(true);  
            else
                RevealObjectIsInteractable(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Player"))
        {
            _useShop = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}