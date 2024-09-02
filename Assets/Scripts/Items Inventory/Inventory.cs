using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<ItemData> StartingEquipment;

    public List<InventoryItem> _inventoryList = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> _inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> _stashList = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> _stashDictionary = new();

    public List<InventoryItem> _equipmentList = new();
    public Dictionary<ItemDataEquipment, InventoryItem> _equipmentDictionary = new();


    [Header("Inventory UI")]
    [SerializeField] private Transform _inventorySlotParent;
    [SerializeField] private Transform _stastSlotParent;
    [SerializeField] private Transform _equipmentSlotParent;
    [SerializeField] private Transform _statSlotParent;


    private UI_ItemSlot[] _inventoryItemSlotArray;
    private UI_ItemSlot[] _stashItemSlotArray;
    private UI_EquipmentSlot[] _equipmentSlotArray;
    private UI_StatSlot[] _statSlotArray;

    [Header("Items Cooldown")]
    private float _lastTimeUsedFlask = Mathf.NegativeInfinity;
    private float _lastTimeUsedArmor = Mathf.NegativeInfinity;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _inventoryList = new();
        _inventoryDictionary = new();
        _stashList = new();
        _stashDictionary = new();
        _equipmentList = new();
        _equipmentDictionary = new();
        _inventoryItemSlotArray = _inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        _stashItemSlotArray = _stastSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        _equipmentSlotArray = _equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        _statSlotArray = _statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        for (int i = 0; i < StartingEquipment.Count; i++)
        {
            if (StartingEquipment[i] != null)
                AddItem(StartingEquipment[i]);
        }
    }
    public bool CanAddItem()
    {
        if (_inventoryList.Count >= _inventoryItemSlotArray.Length)
        {
            return false;
        }
        return true;
    }
    public void AddItem(ItemData itemData)
    {
        if (itemData.ItemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(itemData);
        }
        else if (itemData.ItemType == ItemType.Material)
        {
            AddToStash(itemData);
        }
        UpdateUISlots();
    }
    private void AddToStash(ItemData itemData)
    {
        if (_stashDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newInventoryItem = new InventoryItem(itemData);
            _stashList.Add(newInventoryItem);
            _stashDictionary.Add(itemData, newInventoryItem);
        }
    }
    private void AddToInventory(ItemData itemData)
    {
        if (_inventoryDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newInventoryItem = new InventoryItem(itemData);
            _inventoryList.Add(newInventoryItem);
            _inventoryDictionary.Add(itemData, newInventoryItem);
        }
    }
    public void RemoveItem(ItemData itemData)
    {
        if (_inventoryDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            if (value.StackSize <= 1)
            {
                _inventoryList.Remove(value);
                _inventoryDictionary.Remove(itemData);
            }
            else
            {
                value.RemoveStack();
            }
        }

        if (_stashDictionary.TryGetValue(itemData, out InventoryItem stashValue))
        {
            if (stashValue.StackSize <= 1)
            {
                _stashList.Remove(stashValue);
                _stashDictionary.Remove(itemData);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateUISlots();
    }
    private void UpdateUISlots()
    {
        for (int i = 0; i < _equipmentSlotArray.Length; i++)
        {
            _equipmentSlotArray[i].CleanUpSlot();
        }
        for (int i = 0; i < _inventoryItemSlotArray.Length; i++)
        {
            _inventoryItemSlotArray[i].CleanUpSlot();
        }
        for (int i = 0; i < _stashItemSlotArray.Length; i++)
        {
            _stashItemSlotArray[i].CleanUpSlot();
        }
        for (int i = 0; i < _inventoryList.Count; i++)
        {
            _inventoryItemSlotArray[i].UpdateSlot(_inventoryList[i]);
        }
        for (int i = 0; i < _stashList.Count; i++)
        {
            _stashItemSlotArray[i].UpdateSlot(_stashList[i]);
        }
        for (int i = 0; i < _equipmentSlotArray.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in _equipmentDictionary)
            {
                if (item.Key.EquipmentType == _equipmentSlotArray[i].SlotType)
                {
                    _equipmentSlotArray[i].UpdateSlot(item.Value);
                }
            }
        }
        for (int i = 0; i < _statSlotArray.Length; i++)
        {
            _statSlotArray[i].UpdateStatValueUI();
        }
    }

    public void EquipItem(ItemData itemData)
    {
        ItemDataEquipment newEquipmentItem = itemData as ItemDataEquipment;
        InventoryItem newInventoryItem = new(newEquipmentItem);

        ItemDataEquipment oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in _equipmentDictionary)
        {
            if (item.Key.EquipmentType == newEquipmentItem.EquipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        _equipmentList.Add(newInventoryItem);
        _equipmentDictionary.Add(newEquipmentItem, newInventoryItem);
        RemoveItem(itemData);
        newEquipmentItem.AddModifiers();


        UpdateUISlots();
    }

    internal void UnequipItem(ItemDataEquipment equipmentToRemove)
    {
        if (_equipmentDictionary.TryGetValue(equipmentToRemove, out InventoryItem value))
        {
            _equipmentList.Remove(value);
            _equipmentDictionary.Remove(equipmentToRemove);
            equipmentToRemove.RemoveModifiers();
        }
    }

    public bool CanCraft(ItemDataEquipment itemWantedCrafted)
    {
        List<InventoryItem> requiredMaterialList = itemWantedCrafted.CraftingMaterialList;
        for (int i = 0; i < requiredMaterialList.Count; i++)
        {
            if (_stashDictionary.TryGetValue(requiredMaterialList[i].Data, out InventoryItem stashValue))
            {
                if (stashValue.StackSize < requiredMaterialList[i].StackSize)
                {
                    Debug.Log("not enough material " + stashValue.Data.name);
                    return false;
                }
            }
            else
            {
                Debug.Log("not enough material");
                return false;
            }
        }

        CraftItem(itemWantedCrafted);
        return true;
    }

    internal void CraftItem(ItemDataEquipment craftData)
    {
        for (int i = 0; i < craftData.CraftingMaterialList.Count; i++)
        {
            RemoveItem(craftData.CraftingMaterialList[i].Data);
        }
        AddItem(craftData);
        Debug.Log("item crafted " + craftData.name);
    }

    public List<InventoryItem> GetEquipmentList()
    {
        return _equipmentList;
    }
    public List<InventoryItem> GetStashList()
    {
        return _stashList;
    }

    public ItemDataEquipment GetEquipment(EquipmentType equipmentType)
    {
        ItemDataEquipment equippedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in _equipmentDictionary)
        {
            if (item.Key.EquipmentType == equipmentType)
                equippedItem = item.Key;
        }

        return equippedItem;
    }
    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);
        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > _lastTimeUsedFlask + currentFlask.ItemCooldown;
        if (canUseFlask)
        {
            Debug.Log("Flask effect used");
            currentFlask.ExecuteItemEffect(null);
            _lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask on CD");
        }
    }
    public bool CanUseArmor()
    {
        ItemDataEquipment currentArmor = GetEquipment(EquipmentType.Armor);
        if (Time.time > _lastTimeUsedArmor + currentArmor.ItemCooldown)
        {
            _lastTimeUsedArmor = Time.time;
            return true;
        }

        Debug.Log("Armor in cooldown");
        return false;
    }
}