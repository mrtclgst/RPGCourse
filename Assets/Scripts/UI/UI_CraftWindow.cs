using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemDescription;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image[] _materialImageArray;
    [SerializeField] private Button _craftButton;

    public void SetupCraftWindow(ItemDataEquipment itemDataEquipment)
    {
        _craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < _materialImageArray.Length; i++)
        {
            _materialImageArray[i].color = Color.clear;
            _materialImageArray[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < itemDataEquipment.CraftingMaterialList.Count; i++)
        {

            if (itemDataEquipment.CraftingMaterialList.Count > _materialImageArray.Length)
                Debug.LogWarning("you have more mateirals amount than you have material slots in craft panel");


            _materialImageArray[i].sprite = itemDataEquipment.CraftingMaterialList[i].Data.Icon;
            _materialImageArray[i].color = Color.white;

            TextMeshProUGUI materialSlotText = _materialImageArray[i].GetComponentInChildren<TextMeshProUGUI>();
            materialSlotText.color = Color.white;
            materialSlotText.text = itemDataEquipment.CraftingMaterialList[i].StackSize.ToString();

        }

        _itemIcon.sprite = itemDataEquipment.Icon;
        _itemName.text = itemDataEquipment.ItemName;
        _itemDescription.text = itemDataEquipment.GetDescription();

        _craftButton.onClick.AddListener(() => Inventory.Instance.CanCraft(itemDataEquipment));
    }
}
