using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType SlotType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (Item == null || Item.Data == null) return;

        //base.OnPointerDown(eventData);
        Inventory.Instance.UnequipItem(Item.Data as ItemDataEquipment);
        Inventory.Instance.AddItem(Item.Data as ItemDataEquipment);
        _ui.ItemTooltip.HideTooltip();
        CleanUpSlot();
    }
}