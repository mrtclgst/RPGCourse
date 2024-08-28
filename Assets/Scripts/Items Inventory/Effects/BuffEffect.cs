using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class BuffEffect : ItemEffect
{
    private PlayerStats _playerStats;
    [SerializeField] private StatType _buffType = StatType.Health;
    [SerializeField] private int _buffAmount;
    [SerializeField] private int _buffDuration;

    public override void ExecuteEffect(Transform targetTransform)
    {
        _playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();
        _playerStats.IncreaseStatBy(_buffAmount, _buffDuration, StatFromType());
    }

    private Stat StatFromType()
    {
        switch (_buffType)
        {
            case StatType.Strength: return _playerStats.Strength;
            case StatType.Agility: return _playerStats.Agility;
            case StatType.Intelligence: return _playerStats.Intelligence;
            case StatType.Vitality: return _playerStats.Vitality;
            case StatType.Damage: return _playerStats.Damage;
            case StatType.CritChance: return _playerStats.CritChance;
            case StatType.CritDamage: return _playerStats.CritDamage;
            case StatType.Health: return _playerStats.MaxHealth;
            case StatType.Armor: return _playerStats.Armor;
            case StatType.Evasion: return _playerStats.Evasion;
            case StatType.MagicResistance: return _playerStats.MagicResistance;
            case StatType.FireDamage: return _playerStats.FireDamage;
            case StatType.IceDamage: return _playerStats.IceDamage;
            case StatType.LightningDamage: return _playerStats.LightningDamage;
        }
        return null;
    }
}

public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,
    Damage,
    CritChance,
    CritDamage,
    Health,
    Armor,
    Evasion,
    MagicResistance,
    FireDamage,
    IceDamage,
    LightningDamage,
}