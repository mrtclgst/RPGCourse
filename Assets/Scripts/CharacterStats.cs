using System.Runtime.Serialization;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    [SerializeField] private Stat _strength;    // 1 point increase damage by 1 and crit damage 1%
    [SerializeField] private Stat _agility;     // 1 point increase evasion by 1% and crit chance 1%
    [SerializeField] private Stat _intelligence;// 1 point increase magic damage 1 and magic resistance by 3
    [SerializeField] private Stat _vitality;    // 1 point increase health 3 or 5 points

    [Header("Offensive Stats")]
    [SerializeField] private Stat _damage;
    [SerializeField] private Stat _critChance;
    [SerializeField] private Stat _critDamage;      //default value 150

    [Header("Defensive Stats")]
    [SerializeField] private Stat _maxHealth;
    [SerializeField] private Stat _armor;
    [SerializeField] private Stat _evasion;

    private int _currentHealth;

    protected virtual void Start()
    {
        _currentHealth = _maxHealth.GetValue();
        _critDamage.SetBaseValue(150);
    }

    internal virtual void DealDamage(CharacterStats targetStats)
    {
        Debug.Log("attacked to" + gameObject.name);
        int totalDamage = _damage.GetValue() + _strength.GetValue();
        if (CanCrit())
        {
            Debug.Log("critical hit");
            totalDamage = CriticalDamage(totalDamage);
            Debug.Log(totalDamage);
        }

        targetStats.TakeDamage(totalDamage);
    }
    internal virtual void TakeDamage(int damage)
    {
        if (CanAvoidAttack())
            return;

        _currentHealth = _currentHealth - ArmorAddedDamage(damage);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    private int ArmorAddedDamage(int damage)
    {
        int damageArmored = Mathf.Max(damage - _armor.GetValue(), 0);
        return damageArmored;
    }
    private bool CanCrit()
    {
        int totalCritChance = _critChance.GetValue() + _agility.GetValue();
        Debug.Log($"{totalCritChance}");
        if (Random.Range(0, 100) < totalCritChance)
        {
            return true;
        }
        return false;
    }
    private int CriticalDamage(int damage)
    {
        float totalCritDamage = (_critDamage.GetValue() + _strength.GetValue()) / 100f;
        float critDamage = damage * totalCritDamage;
        return Mathf.RoundToInt(critDamage);
    }
    private bool CanAvoidAttack()
    {
        int totalEvasion = _evasion.GetValue() + _agility.GetValue();
        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
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