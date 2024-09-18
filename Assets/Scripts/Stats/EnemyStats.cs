using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy _enemy;
    private ItemDrop _itemDrop;
    public Stat SoulsDropAmount;

    [Header("Level Details")]
    [SerializeField] private int _level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float _percentageModifier = 0.1f;

    protected override void Start()
    {
        SoulsDropAmount.SetBaseValue(100);
        ApplyLevelModifiers();
        base.Start();
        _enemy = GetComponent<Enemy>();
        _itemDrop = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(Strength);
        Modify(Agility);
        Modify(Intelligence);
        Modify(Vitality);

        Modify(Damage);
        Modify(CritChance);
        Modify(CritDamage);

        Modify(MaxHealth);
        Modify(Armor);
        Modify(Evasion);
        Modify(MagicResistance);

        Modify(FireDamage);
        Modify(IceDamage);
        Modify(LightningDamage);

        Modify(SoulsDropAmount);
    }

    internal override void DealDamage(CharacterStats targetStats)
    {
        base.DealDamage(targetStats);
    }
    internal override void TakeDamage(int damage, bool isShocked)
    {
        base.TakeDamage(damage, isShocked);
    }
    protected override void Die()
    {
        base.Die();
        _enemy.Die();
        _itemDrop.GenerateDrop();
        PlayerManager.Instance.Currency += SoulsDropAmount.GetValue();
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < _level; i++)
        {
            float modifier = _stat.GetValue() * _percentageModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
}