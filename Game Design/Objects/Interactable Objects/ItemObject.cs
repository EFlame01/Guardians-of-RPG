using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ItemObject is a class that extends the
/// InteractableObject class. ItemObjects
/// allow the player to obtain a list of items
/// it finds inside the object and place them
/// in their inventory.
/// </summary>
public class ItemObject : InteractableObject, IDialogue
{
    [SerializeField] public string itemID;
    [SerializeField] public string[] Items;
    [SerializeField] public int[] AmountsPerItem;
    [SerializeField] public ObjectSprite ItemSprite;
    [SerializeField] private GameObject _textBox;
    [SerializeField] private DialogueData _dialogueData;
    
    private bool _openedItem;
    private ItemData _itemData;

    public void OnEnable()
    {
        _itemData = ItemDataContainer.GetItemData(itemID) ?? new ItemData(itemID, false);
        
        if(_itemData.Opened)
            Destroy(gameObject);
    }

    /// <summary>
    /// If player can interact, calls method
    /// OpenItemObject().
    /// </summary>
    public override void InteractWithObject()
    {
        if(CanInteract && !_openedItem)
        {
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            _openedItem = true;
            StartCoroutine(OpenItemObect());
        }
    }

    /// <summary>
    /// Starts animation to open object,
    /// and start dialogue.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpenItemObect()
    {
        _openedItem = true;
        ItemSprite.OpenAnimation();
        yield return new WaitForSeconds(0.4f);
        StartDialogue();
        _itemData.UpdateItemData(true);
        CheckForInteraction = true;
    }

    /// <summary>
    /// Opens dialogue box to let user know
    /// what items were found and where they 
    /// are located in their inventory.
    /// </summary>
    public void StartDialogue()
    {
        TextBox textBox = Instantiate(_textBox).GetComponent<TextBox>();
        Item[] items = ConvertListToItems();

        for(int i = 0; i < Items.Length; i++)
            PlaceItemsInBag(items[i], AmountsPerItem[i]);
        
        textBox.OpenTextBox();
        textBox.StartNarration(_dialogueData);
    }

    /// <summary>
    /// Places item and specified amount in inventory.
    /// If amount is 0, it will default to 1.
    /// </summary>
    /// <param name="item">the item being imported</param>
    /// <param name="amount">the amount of said items being imported</param>
    private void PlaceItemsInBag(Item item, int amount)
    {
        int trueAmount = Mathf.Max(1, amount);
        Player.Instance().Inventory.AddItem(item.Name, trueAmount);
    }

    /// <summary>
    /// Converts the <c>Items</c> variable into a list
    /// of items.
    /// </summary>
    /// <returns>an array of type <c>Item</c></returns>
    private Item[] ConvertListToItems()
    {   
        List<Item> list = new List<Item>();

        foreach(string item in Items)
            list.Add(ItemMaker.Instance.GetItemBasedOnName(item));

        return list.ToArray();
    }

    private void OnCollisionEnter2D(Collision2D collider2D)
    {
        if(_openedItem)
            return;

        if(ObjectDetected)
            return;

        if(collider2D.gameObject.tag.Equals("Player"))
            RevealObjectIsInteractable(true);
    }

    private void OnCollisionStay2D(Collision2D collider2D)
    {
        if(_openedItem)
            return;
        
        if(!ObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if(IsThisObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Player"))
        {
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}