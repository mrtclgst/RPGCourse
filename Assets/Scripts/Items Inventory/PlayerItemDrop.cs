using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float _chanceToLoseItems;
    [SerializeField] private float _chanceToLoseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.Instance;

        List<InventoryItem> itemsToUnequip = new();
        List<InventoryItem> materialsToLose = new();

        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= _chanceToLoseItems)
            {
                DropItem(item.Data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].Data as ItemDataEquipment);
        }


        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= _chanceToLoseMaterials)
            {
                DropItem(item.Data);
                materialsToLose.Add(item);
            }
        }

        for (int i = 0; i < materialsToLose.Count; i++)
        {
            inventory.RemoveItem(materialsToLose[i].Data);
        }
    }
}
