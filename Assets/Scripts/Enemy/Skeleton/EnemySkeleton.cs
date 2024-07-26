using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region States
    public SkeletonIdleState IdleState { get; private set; }
    public SkeletonMoveState MoveState { get; private set; }
    public SkeletonBattleState BattleState { get; private set; }
    public SkeletonAttackState AttackState { get; private set; }
    public SkeletonStunnedState StunnedState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        IdleState = new SkeletonIdleState(this, StateMachine, "Idle", this);
        MoveState = new SkeletonMoveState(this, StateMachine, "Move", this);
        BattleState = new SkeletonBattleState(this, StateMachine, "Move", this);
        AttackState = new SkeletonAttackState(this, StateMachine, "Attack", this);
        StunnedState = new SkeletonStunnedState(this, StateMachine, "Stunned", this);
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
    }
    public override void DealDamage()
    {
        Collider2D[] damageableArray = Physics2D.OverlapCircleAll(_attackCheckPoint.position, _attackCheckRadius);
        foreach (var damageable in damageableArray)
        {
            if (damageable.GetComponent<Player>() != null)
            {
                damageable.GetComponent<Player>().TakeDamage();
            }
        }

        base.DealDamage();
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
}