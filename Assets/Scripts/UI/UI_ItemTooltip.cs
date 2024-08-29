using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemTypeText;
    [SerializeField] private TextMeshProUGUI _itemDecriptionText;

    public void ShowTooltip(ItemDataEquipment item)
    {
        if (item == null) return;

        _itemNameText.text = item.ItemName;
        _itemTypeText.text = item.EquipmentType.ToString();
        _itemDecriptionText.text = item.GetDescription();
        gameObject.SetActive(true);
    }
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
