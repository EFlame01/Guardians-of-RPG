using System.Collections;
using UnityEngine;

public class ShopObject : InteractableObject
{
    [SerializeField] private PlayerDirection _directionToUseShop;
    [SerializeField] private string shopName;
    [SerializeField] private ShopList shopList;
    [SerializeField] private ShopMenu shopMenu;

    private bool _useShop;

    public override void InteractWithObject()
    {
        if(CanInteract && !_useShop)
        {
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            _useShop = true;
            OpenShop();
        }
    }

    private void OpenShop()
    {
        //TODO: open shop menu
        //TODO: pause game
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