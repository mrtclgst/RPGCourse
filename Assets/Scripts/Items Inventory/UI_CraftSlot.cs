using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(Item);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemDataEquipment craftData = Item.Data as ItemDataEquipment;

        if (Inventory.Instance.CanCraft(craftData))
        {
            Inventory.Instance.CraftItem(craftData);
        }
    }
}