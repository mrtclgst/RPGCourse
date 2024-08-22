using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]

public class ItemDataEquipment : ItemData
{
    public EquipmentType EquipmentType;
}

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask,
}