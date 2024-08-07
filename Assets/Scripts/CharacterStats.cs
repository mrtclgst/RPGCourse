using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private Stat _damage;
    [SerializeField] private Stat _maxHealth;
    [SerializeField] private Stat _strength;


    private int _currentHealth;

    protected virtual void Start()
    {
        _currentHealth = _maxHealth.GetValue();
    }


    internal virtual void DealDamage(CharacterStats targetStats)
    {
        int totalDamage = _damage.GetValue() + _strength.GetValue();
        targetStats.TakeDamage(totalDamage);
    }

    internal virtual void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
    }


    internal int GetDamage()
    {
        return _damage.GetValue();
    }
    internal int GetCurrentHealth()
    {
        return _currentHealth;
    }
}