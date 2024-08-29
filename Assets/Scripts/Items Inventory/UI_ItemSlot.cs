using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemText;

    public InventoryItem Item;
    public void UpdateSlot(InventoryItem inventoryItem)
    {
        Item = inventoryItem;
        _itemImage.color = Color.white;
        if (Item != null)
        {
            _itemImage.sprite = Item.Data.Icon;
            if (Item.StackSize > 1)
            {
                _itemText.text = Item.StackSize.ToString();
            }
            else
            {
                _itemText.text = "";
            }
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Item == null) return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.Instance.RemoveItem(Item.Data);
            return;
        }

        if (Item.Data.ItemType == ItemType.Equipment)
            Inventory.Instance.EquipItem(Item.Data);
    }
    public void CleanUpSlot()
    {
        Item = null;
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemText.text = "";
    }
}