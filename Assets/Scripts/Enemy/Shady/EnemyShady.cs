using System;
using UnityEngine;

public class EnemyShady : Enemy
{
    [Header("Shady Specific")]
    [SerializeField] internal float _battleMoveSpeed = 5f;
    [SerializeField] private GameObject _explosive;
    [SerializeField] private float _growSpeed;
    [SerializeField] private float _maxSize;

    #region States
    public ShadyIdleState IdleState { get; private set; }
    public ShadyMoveState MoveState { get; private set; }
    public ShadyDeadState DeadState { get; private set; }
    public ShadyStunnedState StunnedState { get; private set; }
    public ShadyBattleState BattleState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        IdleState = new(this, StateMachine, "Idle", this);
        MoveState = new(this, StateMachine, "Move", this);
        DeadState = new(this, StateMachine, "Dead", this);
        StunnedState = new(this, StateMachine, "Stunned", this);
        BattleState = new(this, StateMachine, "MoveFast", this);
    }
    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }
    internal override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            StateMachine.ChangeState(StunnedState);
            return true;
        }

        return false;
    }

    internal override void Die()
    {
        base.Die();
        StateMachine.ChangeState(DeadState);
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newExplosive = Instantiate(_explosive, _attackCheckPoint.position, Quaternion.identity);
        newExplosive.GetComponent<ShadyExplosiveController>().SetupExplosive(Stats, _growSpeed, _maxSize, _attackCheckRadius);
        Collider.enabled = false;
        RB.gravityScale = 0;
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
