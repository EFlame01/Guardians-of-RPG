using System.Collections;
using UnityEngine;

public class ShopObject : InteractableObject
{
    [SerializeField] private PlayerDirection _directionToReadSign;
    [SerializeField] ShopList shopList;
    [SerializeField] ShopMenu shopMenu;

    public override void InteractWithObject()
    {
        if(!ObjectDetected)
        {
            if(GetComponent<Collider2D>().gameObject.tag.Equals("Player"))
                RevealObjectIsInteractable(true);
        }
        else if(IsThisObjectDetected)
        {

        }
        else
            RevealObjectIsInteractable(false);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {

    }
    
    private void OnTriggerStay2D(Collider2D collider2D)
    {

    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {

    }
}