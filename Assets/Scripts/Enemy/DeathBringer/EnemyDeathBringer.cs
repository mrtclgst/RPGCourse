using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyDeathBringer : Enemy
{
    #region States
    public DeathBringerAttackState AttackState { get; private set; }
    public DeathBringerBattleState BattleState { get; private set; }
    public DeathBringerDeadState DeadState { get; private set; }
    public DeathBringerIdleState IdleState { get; private set; }
    public DeathBringerSpellCastState SpellCastState { get; private set; }
    public DeathBringerTeleportState TeleportState { get; private set; }
    #endregion

    [Header("Teleport Details")]
    [SerializeField] private BoxCollider2D _arenaCd;
    [SerializeField] private Vector2 _surroundingCheckSize;
    [SerializeField] private float _chanceToTeleport;
    [SerializeField] private float _defaultChanceToTeleport = 25f;

    [Header("Spell Cast Details")]
    [SerializeField] private GameObject _spellPrefab;
    [SerializeField] private int _amountOfSpells;
    [SerializeField] private float _castCooldown;
    [SerializeField] private float _spellCastStateCooldown;
    private float _lastTimeCast;

    private bool _bossFightBegun;

    protected override void Awake()
    {
        base.Awake();
        AttackState = new(this, StateMachine, "Attack", this);
        BattleState = new(this, StateMachine, "Move", this);
        DeadState = new(this, StateMachine, "Idle", this);
        IdleState = new(this, StateMachine, "Idle", this);
        SpellCastState = new(this, StateMachine, "SpellCast", this);
        TeleportState = new(this, StateMachine, "Teleport", this);
    }
    protected override void Start()
    {
        base.Start();
        SetupDefaultDirection(-1);
        StateMachine.Initialize(IdleState);
        _chanceToTeleport = _defaultChanceToTeleport;
    }
    internal override void Die()
    {
        base.Die();
        StateMachine.ChangeState(DeadState);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, _surroundingCheckSize);
    }
    private RaycastHit2D GroundBelow()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 100, _whatIsGround);
    }
    private bool IsSomethingAround()
    {
        return Physics2D.BoxCast(transform.position, _surroundingCheckSize, 0, Vector2.zero, 0, _whatIsGround);
    }
    public void FindPosition()
    {
        float x = Random.Range(_arenaCd.bounds.min.x + 3, _arenaCd.bounds.max.x - 3);
        float y = Random.Range(_arenaCd.bounds.min.y + 3, _arenaCd.bounds.max.y - 3);
        transform.position = new Vector3(x, y, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + Collider.size.y / 2);
        if (!GroundBelow() || IsSomethingAround())
        {
            Debug.Log("Looking for new pos");
            FindPosition();
        }
    }
    public bool CanTeleport()
    {
        if (Random.Range(0, 100) <= _chanceToTeleport)
        {
            _chanceToTeleport = _defaultChanceToTeleport;
            return true;
        }

        return false;
    }
    internal void IncreaseTeleportChance(float addition)
    {
        _chanceToTeleport += addition;
    }
    public bool CanCastSpell()
    {
        if (Time.time >= _lastTimeCast + _spellCastStateCooldown)
        {
            _lastTimeCast = Time.time;
            return true;
        }

        return false;
    }
    internal void CastSpell()
    {
        Player player = PlayerManager.Instance.Player;
        Vector3 spellCastPosition = new Vector3(player.transform.position.x + Random.Range(-2, 2), player.transform.position.y + 1.4f);
        GameObject newSpell = Instantiate(_spellPrefab, spellCastPosition, Quaternion.identity);
        newSpell.GetComponent<DeathBringerSpellCastController>().SetupSpell(Stats);
    }
    internal int GetAmountOfSpells()
    {
        return _amountOfSpells;
    }
    internal float GetSpellCooldown()
    {
        return _castCooldown;
    }
    public override void DealDamage()
    {
        base.DealDamage();
        Collider2D[] damageableArray = Physics2D.OverlapCircleAll(_attackCheckPoint.position, _attackCheckRadius);
        foreach (var damageable in damageableArray)
        {
            if (damageable.GetComponent<Player>() != null)
            {
                Player player = damageable.GetComponent<Player>();
                Stats.DealDamage(player.Stats);
            }
        }
    }

    internal void SetBossFightBegun(bool fightBegun)
    {
        _bossFightBegun = fightBegun;
    }
    internal void SetLastTimeCast(float lastTimeSpellCasted)
    {
        _lastTimeCast = lastTimeSpellCasted;
    }

    internal bool GetBossFightBegun()
    {
        return _bossFightBegun;
    }
}
