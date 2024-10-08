using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image _itemImage;
    [SerializeField] protected TextMeshProUGUI _itemText;

    public InventoryItem Item;

    protected UI _ui;

    protected virtual void Start()
    {
        _ui = GetComponentInParent<UI>();
    }

    public virtual void UpdateSlot(InventoryItem inventoryItem)
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

        _ui.ItemTooltip.HideTooltip();
    }
    public void CleanUpSlot()
    {
        Item = null;
        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemText.text = "";
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Item == null) return;

        _ui.ItemTooltip.ShowTooltip(Item.Data as ItemDataEquipment);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(Item == null) return;
     
        _ui.ItemTooltip.HideTooltip();
    }
}