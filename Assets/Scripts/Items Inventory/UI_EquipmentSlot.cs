using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType SlotType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        //base.OnPointerDown(eventData);
        Inventory.Instance.UnequipItem(Item.Data as ItemDataEquipment);
        Inventory.Instance.AddItem(Item.Data as ItemDataEquipment);
        CleanUpSlot();
    }
}