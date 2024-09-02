using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform _craftSlotParent;
    [SerializeField] private GameObject _craftSlotPrefab;

    [SerializeField] private List<ItemDataEquipment> _craftEquipment;

    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
    }

    public void SetupCraftList()
    {
        for (int i = 0; i < _craftSlotParent.childCount; i++)
        {
            Destroy(_craftSlotParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < _craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(_craftSlotPrefab, _craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(_craftEquipment[i]);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }
    public void SetupDefaultCraftWindow()
    {
        if (_craftEquipment[0] != null)
        {
            GetComponentInParent<UI>().CraftWindow.SetupCraftWindow(_craftEquipment[0]);
        }
    }
}
