using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

/// <summary>
/// ItemObject is a class that extends the
/// InteractableObject class. ItemObjects
/// allow the player to obtain a list of items
/// it finds inside the object and place them
/// in their inventory.
/// </summary>
public class ItemObject : InteractableObject, IDialogue
{
    //public variables
    [System.Serializable]
    public struct ItemObjectStruct
    {
        public string itemName;
        public int itemAmount;
    }

    //Serialized variables
    [SerializeField] private string _itemID;
    [SerializeField] private ItemObjectStruct[] _items;
    [SerializeField] private ObjectSprite _itemSprite;
    [SerializeField] private DialogueData _itemLootSingular;
    [SerializeField] private DialogueData _itemLootPlural;

    //private static variables
    private static WaitForSeconds _waitForSeconds0_4 = new WaitForSeconds(0.4f);

    //private variables
    private DialogueData _dialogueData;
    private bool _openedItem;
    private ItemData _itemData;

    public void OnEnable()
    {
        _itemData = ItemDataContainer.GetItemData(_itemID) ?? new ItemData(_itemID, false);

        if (_itemData.Opened)
            Destroy(gameObject);
    }

    /// <summary>
    /// If player can interact, calls method
    /// OpenItemObject().
    /// </summary>
    public override void InteractWithObject()
    {
        if (CanInteract && !_openedItem)
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
        _itemSprite.OpenAnimation();
        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.RECIEVED);
        yield return _waitForSeconds0_4;
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
        int typesOfItems = 0;
        Item[] items = new Item[_items.Length];
        foreach (ItemObjectStruct i in _items)
        {
            Item item = ItemMaker.Instance.GetItemBasedOnName(i.itemName);
            PlaceItemsInBag(item, i.itemAmount);
            items[typesOfItems] = item;
            typesOfItems++;
        }

        if (typesOfItems == 1)
        {
            _dialogueData = _itemLootSingular;
            DialogueManager.Instance.CurrentStory = new Story(_dialogueData.InkJSON.text);
            DialogueManager.Instance.CurrentStory.variablesState["itemAmount"] = _items[0].itemAmount;
            DialogueManager.Instance.CurrentStory.variablesState["itemName"] = _items[0].itemAmount > 1 ? items[0].PluralName : items[0].Name;
            DialogueManager.Instance.CurrentStory.variablesState["itemType"] = items[0].Type.ToString();
        }
        else if (typesOfItems > 1)
        {
            _dialogueData = _itemLootPlural;
            DialogueManager.Instance.CurrentStory = new Story(_dialogueData.InkJSON.text);
            string listItems = "";
            for (int i = 0; i < items.Length; i++)
            {
                int itemAmount = _items[i].itemAmount;
                string itemName = itemAmount > 1 ? items[i].PluralName : items[i].Name; ;
                if (i + 1 == items.Length)
                    listItems += "and " + itemAmount + " " + itemName;
                else
                    listItems += itemAmount + " " + itemName + ", ";
            }
            DialogueManager.Instance.CurrentStory.variablesState["listItems"] = listItems;
        }

        DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
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

        foreach (ItemObjectStruct item in _items)
            list.Add(ItemMaker.Instance.GetItemBasedOnName(item.itemName));

        return list.ToArray();
    }

    private void OnCollisionEnter2D(Collision2D collider2D)
    {
        if (_openedItem)
            return;

        if (ObjectDetected)
            return;

        if (collider2D.gameObject.CompareTag("Player"))
            RevealObjectIsInteractable(true);
    }

    private void OnCollisionStay2D(Collision2D collider2D)
    {
        if (_openedItem)
            return;

        if (!ObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if (IsThisObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player"))
        {
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}