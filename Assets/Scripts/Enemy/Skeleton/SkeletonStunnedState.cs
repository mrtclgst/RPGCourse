using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private EnemySkeleton _enemySkeleton;
    public SkeletonStunnedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton)
        : base(enemy, stateMachine, animBoolName)
    {
        _enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.EntityFX.InvokeRepeating("RedColorBlink", 0, 0.1f);
        _stateTimer = _enemy.GetStunDuration();
        Vector2 stunForce = _enemy.GetStunForce();
        _rb.velocity = new Vector2(-1 * _enemy.GetFacingDirection() * stunForce.x, stunForce.y);
    }

    public override void Exit()
    {
        base.Exit();
        _enemy.EntityFX.Invoke("CancelRedBlink", 0);
    }

    public override void Update()
    {
        base.Update();
        if (_stateTimer < 0)
        {
            _stateMachine.ChangeState(_enemySkeleton.IdleState);
        }
    }
}