using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemDataEquipment data)
    {
        if (data == null)
            return;

        Item.Data = data;
        _itemImage.sprite = data.Icon;
        _itemText.text = data.ItemName;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        _ui.CraftWindow.SetupCraftWindow(Item.Data as ItemDataEquipment);
    }
}