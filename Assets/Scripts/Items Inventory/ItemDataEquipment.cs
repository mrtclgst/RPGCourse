using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]

public class ItemDataEquipment : ItemData
{
    public EquipmentType EquipmentType;
    public ItemEffect[] ItemEffectArray;

    [Header("Main Stats")]
    public int Strength;
    public int Agility;
    public int Intelligence;
    public int Vitality;

    [Header("Offensive Stats")]
    public int Damage;
    public int CritChance;
    public int CritDamage;

    [Header("Defensive Stats")]
    public int Health;
    public int Armor;
    public int Evasion;
    public int MagicResistance;

    [Header("Magic Stats")]
    public int FireDamage;
    public int IceDamage;
    public int LightningDamage;

    [Header("Craft Requirements")]
    public List<InventoryItem> CraftingMaterialList;

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        playerStats.Strength.AddModifier(Strength);
        playerStats.Agility.AddModifier(Agility);
        playerStats.Intelligence.AddModifier(Intelligence);
        playerStats.Vitality.AddModifier(Vitality);

        playerStats.Damage.AddModifier(Damage);
        playerStats.CritChance.AddModifier(CritChance);
        playerStats.CritDamage.AddModifier(CritDamage);

        playerStats.MaxHealth.AddModifier(Health);
        playerStats.Armor.AddModifier(Armor);
        playerStats.Evasion.AddModifier(Evasion);
        playerStats.MagicResistance.AddModifier(MagicResistance);

        playerStats.FireDamage.AddModifier(FireDamage);
        playerStats.IceDamage.AddModifier(IceDamage);
        playerStats.LightningDamage.AddModifier(LightningDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        playerStats.Strength.RemoveModifier(Strength);
        playerStats.Agility.RemoveModifier(Agility);
        playerStats.Intelligence.RemoveModifier(Intelligence);
        playerStats.Vitality.RemoveModifier(Vitality);

        playerStats.Damage.RemoveModifier(Damage);
        playerStats.CritChance.RemoveModifier(CritChance);
        playerStats.CritDamage.RemoveModifier(CritDamage);

        playerStats.MaxHealth.RemoveModifier(Health);
        playerStats.Armor.RemoveModifier(Armor);
        playerStats.Evasion.RemoveModifier(Evasion);
        playerStats.MagicResistance.RemoveModifier(MagicResistance);

        playerStats.FireDamage.RemoveModifier(FireDamage);
        playerStats.IceDamage.RemoveModifier(IceDamage);
        playerStats.LightningDamage.RemoveModifier(LightningDamage);
    }

    public void ExecuteItemEffect(Transform enemyTransform)
    {
        foreach (ItemEffect itemEffect in ItemEffectArray)
        {
            itemEffect.ExecuteEffect(enemyTransform);
        }
    }
}

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask,
}