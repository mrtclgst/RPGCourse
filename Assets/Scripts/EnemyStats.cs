using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy _enemy;

    protected override void Start()
    {
        base.Start();
        _enemy = GetComponent<Enemy>();
    }
    internal override void DealDamage(CharacterStats targetStats)
    {
        base.DealDamage(targetStats);
    }
    internal override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
    protected override void Die()
    {
        base.Die();
        _enemy.Die();
    }
}