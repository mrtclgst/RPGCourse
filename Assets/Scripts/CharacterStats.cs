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
    [SerializeField] private Stat _magicResistance;

    [Header("Magic Stats")]
    [SerializeField] private Stat _fireDamage;
    [SerializeField] private Stat _iceDamage;
    [SerializeField] private Stat _lightningDamage;

    [SerializeField] private bool _isIgnited;
    [SerializeField] private bool _isChilled;
    [SerializeField] private bool _isShocked;


    private int _currentHealth;

    protected virtual void Start()
    {
        _currentHealth = _maxHealth.GetValue();
        _critDamage.SetBaseValue(150);
    }
    internal virtual void DealDamage(CharacterStats targetStats)
    {
        int totalDamage = _damage.GetValue() + _strength.GetValue();
        if (CanCrit())
        {
            totalDamage = CriticalDamage(totalDamage);
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
    public virtual void DealMagicalDamage(CharacterStats targetStats)
    {
        int fireDamage = _fireDamage.GetValue();
        int iceDamage = _iceDamage.GetValue();
        int lightningDamage = _lightningDamage.GetValue();
        int totalMagicalDamage = fireDamage + iceDamage + lightningDamage + _intelligence.GetValue();
        totalMagicalDamage = totalMagicalDamage - (targetStats._magicResistance.GetValue() + (targetStats._intelligence.GetValue() * 3));
        totalMagicalDamage = Mathf.Max(totalMagicalDamage, 0);
        targetStats.TakeDamage(totalMagicalDamage);

        int highestMagicalDamage = Mathf.Max(fireDamage, iceDamage, lightningDamage);
        bool canIgnite = false;
        bool canChill = false;
        bool canShock = false;
        if (highestMagicalDamage == fireDamage)
        {
            canIgnite = true;
        }
        else if (highestMagicalDamage == iceDamage)
        {
            canChill = true;
        }
        else if (highestMagicalDamage == lightningDamage)
        {
            canShock = true;
        }
        targetStats.ApplyAilments(canIgnite, canChill, canShock);

    }
    public virtual void ApplyAilments(bool ignite, bool chill, bool shock)
    {
        if (ignite || chill || shock)
        {
            _isIgnited = ignite;
            _isChilled = chill;
            _isShocked = shock;
        }
    }
}