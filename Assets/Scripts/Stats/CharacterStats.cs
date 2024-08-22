using System;
using UnityEditorInternal;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Action CharacterStats_OnHealthChanged;

    private EntityFX _entityFX;

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
    protected bool _isDead;
    private float _igniteTimer;
    private float _igniteDamageCooldown = 0.3f;
    private float _igniteDamageTimer;
    private int _igniteDamage;
    private float _chilledTimer;
    private float _shockedTimer;
    [SerializeField] private GameObject _thunderGO;
    private int _shockDamage;

    protected virtual void OnEnable()
    {
        _currentHealth = _maxHealth.GetValue();
    }
    protected virtual void Start()
    {
        _critDamage.SetBaseValue(150);
        _entityFX = GetComponent<EntityFX>();
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
            DecreaseHealthBy(_igniteDamage);
            if (_currentHealth < 0 && !_isDead)
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
        if (_isIgnited || _isChilled)
            return;

        if (ignite)
        {
            float igniteDuration = 2f;
            _isIgnited = ignite;
            _igniteTimer = igniteDuration;
            _entityFX.IgniteFXFor(igniteDuration);
        }
        if (chill)
        {
            float chillDuration = 4f;
            _isChilled = true;
            _chilledTimer = chillDuration;
            _entityFX.ChillFXFor(chillDuration);
            float slowPercentage = 0.25f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, chillDuration);
        }
        if (shock)
        {
            if (!_isShocked)
            {
                float shockDuration = 4f;
                _isShocked = true;
                _shockedTimer = shockDuration;
                _entityFX.ShockFXFor(shockDuration);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
                float closestDistance = Mathf.Infinity;
                Transform closestEnemy = null;

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                        if (distanceToEnemy < closestDistance && distanceToEnemy > 1)
                        {
                            closestEnemy = hit.transform;
                            closestDistance = distanceToEnemy;
                        }
                    }
                }

                if (closestEnemy == null)
                {
                    closestEnemy = transform;
                }


                if (closestEnemy != null)
                {
                    GameObject thunderStrike = Instantiate(_thunderGO, transform.position, Quaternion.identity);
                    thunderStrike.GetComponent<ThunderStrikeController>().Setup(_shockDamage, closestEnemy.GetComponent<CharacterStats>());
                }

            }
        }

        _isChilled = chill;
        _isShocked = shock;
    }
    internal virtual void TakeDamage(int damage, bool isShocked)
    {
        if (CanAvoidAttack(isShocked))
            return;

        DecreaseHealthBy(ArmorAddedDamage(damage));
        _entityFX.StartCoroutine("IE_FlashFX");
        if (_currentHealth <= 0 && _isDead)
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
        _isDead = true;
    }
    internal int GetDamage()
    {
        return _damage.GetValue();
    }
    internal bool IsDead()
    {
        return _isDead;
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