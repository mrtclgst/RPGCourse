using System;

[Serializable]
public class InventoryItem
{
    public ItemData Data;
    public int StackSize;

    public InventoryItem(ItemData newData)
    {
        Data = newData;
        AddStack();
    }

    public void AddStack()
    {
        StackSize++;
    }
    public void RemoveStack()
    {
        StackSize--;
    }
}