using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Action CharacterStats_OnHealthChanged;

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

    [SerializeField] private bool _isIgnited;   //does damage over time
    [SerializeField] private bool _isChilled;   //decrease armor who is chilled
    [SerializeField] private bool _isShocked;   //reduce accuracy 


    private int _currentHealth;
    private float _igniteTimer;
    private float _igniteDamageCooldown = 0.3f;
    private float _igniteDamageTimer;
    private float _chilledTimer;
    private float _shockedTimer;
    private int _igniteDamage;
    protected virtual void OnEnable()
    {
        _currentHealth = _maxHealth.GetValue();
    }
    protected virtual void Start()
    {
        _critDamage.SetBaseValue(150);
    }
    protected virtual void Update()
    {
        _igniteTimer -= Time.deltaTime;
        _chilledTimer -= Time.deltaTime;
        _shockedTimer -= Time.deltaTime;
        _igniteDamageTimer -= Time.deltaTime;

        if (_igniteTimer < 0)
            _isIgnited = false;
        if (_chilledTimer < 0)
            _isChilled = false;
        if (_shockedTimer < 0)
            _isShocked = false;

        if (_igniteDamageTimer < 0 && _isIgnited)
        {
            _igniteDamageTimer = _igniteDamageCooldown;
            Debug.Log("ignite damage " + _igniteDamage);
            //_currentHealth -= _igniteDamage;
            DecreaseHealthBy(_igniteDamage);
            if (_currentHealth < 0)
                Die();
        }
    }
    internal virtual void DealDamage(CharacterStats targetStats)
    {
        int totalDamage = _damage.GetValue() + _strength.GetValue();
        if (CanCrit())
        {
            totalDamage = CriticalDamage(totalDamage);
        }
        targetStats.TakeDamage(totalDamage, _isShocked);
    }
    public virtual void DealMagicalDamage(CharacterStats targetStats)
    {
        int fireDamage = _fireDamage.GetValue();
        int iceDamage = _iceDamage.GetValue();
        int lightningDamage = _lightningDamage.GetValue();
        int totalMagicalDamage = fireDamage + iceDamage + lightningDamage + _intelligence.GetValue();
        totalMagicalDamage = totalMagicalDamage - (targetStats._magicResistance.GetValue() + (targetStats._intelligence.GetValue() * 3));
        totalMagicalDamage = Mathf.Max(totalMagicalDamage, 0);
        targetStats.TakeDamage(totalMagicalDamage, _isShocked);

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

        if (canIgnite)
        {
            targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDamage * 0.2f));
        }

        targetStats.ApplyAilments(canIgnite, canChill, canShock);
    }
    public virtual void ApplyAilments(bool ignite, bool chill, bool shock)
    {
        if (_isIgnited || _isChilled || _isShocked)
            return;

        if (ignite)
        {
            _isIgnited = ignite;
            _igniteTimer = 2f;
        }
        if (chill)
        {
            _isChilled = true;
            _chilledTimer = 2f;
        }
        if (shock)
        {
            _isShocked = true;
            _shockedTimer = 2f;
        }

        _isChilled = chill;
        _isShocked = shock;
    }
    internal virtual void TakeDamage(int damage, bool isShocked)
    {
        if (CanAvoidAttack(isShocked))
            return;

        //_currentHealth = _currentHealth - ArmorAddedDamage(damage);
        DecreaseHealthBy(ArmorAddedDamage(damage));
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    private int ArmorAddedDamage(int damage)
    {
        if (_isChilled)
        {
            damage = Mathf.Max(damage - Mathf.RoundToInt(_armor.GetValue() * 0.8f), 0);
        }
        else
        {
            damage = Mathf.Max(damage - _armor.GetValue(), 0);
        }

        return damage;
    }
    private bool CanCrit()
    {
        int totalCritChance = _critChance.GetValue() + _agility.GetValue();
        if (UnityEngine.Random.Range(0, 100) < totalCritChance)
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
    private bool CanAvoidAttack(bool isShocked)
    {
        int totalEvasion = _evasion.GetValue() + _agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
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
    public void SetupIgniteDamage(int damage)
    {
        _igniteDamage = damage;
    }
    internal bool IsShocked()
    {
        return _isShocked;
    }
    internal int GetMaxHealth()
    {
        return _maxHealth.GetValue() + _vitality.GetValue() * 5;
    }
    internal float GetCurrentHealthPercentage()
    {
        float percentage = (float)_currentHealth / GetMaxHealth();
        return percentage;
    }
    private void DecreaseHealthBy(int damage)
    {
        _currentHealth -= damage;
        CharacterStats_OnHealthChanged?.Invoke();
    }
}