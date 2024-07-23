using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : EnemyState
{
    private EnemySkeleton _enemySkeleton;

    public SkeletonMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName)
    {
        _enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        _enemy.SetVelocity(_enemy.GetFacingDirection() * _enemy.GetMoveSpeed(), _enemy.RB.velocity.y);
        if (_enemy.IsWallDetected() || !_enemySkeleton.IsGroundDetected())
        {
            _enemy.Flip();
            _stateMachine.ChangeState(_enemySkeleton.IdleState);
        }
    }
}
