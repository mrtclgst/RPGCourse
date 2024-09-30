using System;
using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Action CharacterStats_OnHealthChanged;

    private EntityFX _entityFX;

    [Header("Major Stats")]
    [SerializeField] public Stat Strength;    // 1 point increase damage by 1 and crit damage 1%
    [SerializeField] public Stat Agility;     // 1 point increase evasion by 1% and crit chance 1%
    [SerializeField] public Stat Intelligence;// 1 point increase magic damage 1 and magic resistance by 3
    [SerializeField] public Stat Vitality;    // 1 point increase health 3 or 5 points

    [Header("Offensive Stats")]
    [SerializeField] public Stat Damage;
    [SerializeField] public Stat CritChance;
    [SerializeField] public Stat CritDamage;      //default value 150

    [Header("Defensive Stats")]
    [SerializeField] public Stat MaxHealth;
    [SerializeField] public Stat Armor;
    [SerializeField] public Stat Evasion;
    [SerializeField] public Stat MagicResistance;

    [Header("Magic Stats")]
    [SerializeField] public Stat FireDamage;
    [SerializeField] public Stat IceDamage;
    [SerializeField] public Stat LightningDamage;

    [SerializeField] private bool _isIgnited;   //does damage over time
    [SerializeField] private bool _isChilled;   //decrease armor who is chilled
    [SerializeField] private bool _isShocked;   //reduce accuracy 


    private int _currentHealth;
    protected bool _isDead;
    protected bool _isInvincible;
    protected bool _isVulnurable;
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
        _currentHealth = GetMaxHealth();
    }
    protected virtual void Start()
    {
        CritDamage.SetBaseValue(150);
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
    public virtual void IncreaseStatBy(int modifier, float duration, Stat statToModify)
    {
        StartCoroutine(IE_StatModifyCoroutine(modifier, duration, statToModify));
    }
    private IEnumerator IE_StatModifyCoroutine(int modifier, float duration, Stat statToModify)
    {
        statToModify.AddModifier(modifier);
        yield return new WaitForSeconds(duration);
        statToModify.RemoveModifier(modifier);
    }
    internal virtual void DealDamage(CharacterStats targetStats)
    {
        int totalDamage = Damage.GetValue() + Strength.GetValue();
        bool isCritical = false;
        if (CanCrit())
        {
            totalDamage = CriticalDamage(totalDamage);
            isCritical = true;
        }

        targetStats._entityFX.CreateHitFX(targetStats.transform, isCritical);
        targetStats.GetComponent<Entity>().SetKnockbackDirection(this.transform);
        targetStats.TakeDamage(totalDamage, _isShocked);
    }
    public virtual void DealMagicalDamage(CharacterStats targetStats)
    {
        int fireDamage = FireDamage.GetValue();
        int iceDamage = IceDamage.GetValue();
        int lightningDamage = LightningDamage.GetValue();
        int totalMagicalDamage = fireDamage + iceDamage + lightningDamage + Intelligence.GetValue();
        totalMagicalDamage = totalMagicalDamage - (targetStats.MagicResistance.GetValue() + (targetStats.Intelligence.GetValue() * 3));
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
        if (_isInvincible)
            return;

        if (CanAvoidAttack(isShocked))
            return;

        DecreaseHealthBy(ArmorAddedDamage(damage));
        _entityFX.StartCoroutine("IE_FlashFX");
        
        Entity entity = GetComponent<Entity>();
        if (entity != null)
            entity.KnockbackEffect();

        if (_currentHealth <= 0 && !_isDead)
        {
            Die();
        }
    }
    private int ArmorAddedDamage(int damage)
    {
        if (_isChilled)
        {
            damage = Mathf.Max(damage - Mathf.RoundToInt(Armor.GetValue() * 0.8f), 0);
        }
        else
        {
            damage = Mathf.Max(damage - Armor.GetValue(), 0);
        }

        return damage;
    }
    private bool CanCrit()
    {
        int totalCritChance = CritChance.GetValue() + Agility.GetValue();
        if (UnityEngine.Random.Range(0, 100) < totalCritChance)
        {
            return true;
        }
        return false;
    }
    private int CriticalDamage(int damage)
    {
        float totalCritDamage = (CritDamage.GetValue() + Strength.GetValue()) / 100f;
        float critDamage = damage * totalCritDamage;
        return Mathf.RoundToInt(critDamage);
    }
    private bool CanAvoidAttack(bool isShocked)
    {
        int totalEvasion = Evasion.GetValue() + Agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            OnEvasion();
            return true;
        }
        return false;
    }
    public virtual void OnEvasion()
    {

    }
    protected virtual void Die()
    {
        _isDead = true;
    }
    internal int GetDamage()
    {
        return Damage.GetValue();
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
        return MaxHealth.GetValue() + Vitality.GetValue() * 5;
    }
    internal float GetCurrentHealthPercentage()
    {
        float percentage = (float)_currentHealth / GetMaxHealth();
        return percentage;
    }
    protected virtual void DecreaseHealthBy(int damage)
    {
        int totalDamage = damage;
        if (_isVulnurable)
        {
            totalDamage = Mathf.RoundToInt(damage * 1.1f);
        }

        _currentHealth -= totalDamage;
        CharacterStats_OnHealthChanged?.Invoke();
        _entityFX.CreatePopupText(damage.ToString());
    }
    internal virtual void IncreaseHealthBy(int healAmount)
    {
        _currentHealth += healAmount;
        if (_currentHealth >= GetMaxHealth())
            _currentHealth = GetMaxHealth();

        CharacterStats_OnHealthChanged?.Invoke();
    }
    public void MakeVulnurableFor(float seconds)
    {
        StartCoroutine(IE_VulnurableFor(seconds));
    }
    private IEnumerator IE_VulnurableFor(float seconds)
    {
        _isVulnurable = true;
        yield return new WaitForSeconds(seconds);
        _isVulnurable = false;
    }
    public Stat StatOfType(StatType buffType)
    {
        switch (buffType)
        {
            case StatType.Strength: return Strength;
            case StatType.Agility: return Agility;
            case StatType.Intelligence: return Intelligence;
            case StatType.Vitality: return Vitality;
            case StatType.Damage: return Damage;
            case StatType.CritChance: return CritChance;
            case StatType.CritDamage: return CritDamage;
            case StatType.Health: return MaxHealth;
            case StatType.Armor: return Armor;
            case StatType.Evasion: return Evasion;
            case StatType.MagicResistance: return MagicResistance;
            case StatType.FireDamage: return FireDamage;
            case StatType.IceDamage: return IceDamage;
            case StatType.LightningDamage: return LightningDamage;
        }
        return null;
    }
    public void KillEntity()
    {
        if (!_isDead)
            Die();
    }
    public void MakeInvincible(bool invincibility)
    {
        _isInvincible = invincibility;
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