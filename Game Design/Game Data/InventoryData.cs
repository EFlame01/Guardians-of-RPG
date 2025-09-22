using System;

///<summary>
/// InventoryData is a class that compresses the
/// <c>Inventory</c> information in to an array
/// of strings and ints.
/// </summary>
[Serializable]
public class InventoryData
{
    public string[] ItemNames;
    public int[] ItemAmounts;
}