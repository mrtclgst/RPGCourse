using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton)
        : base(enemy, stateMachine, animBoolName, enemySkeleton)
    {
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
        _enemy.SetVelocity(_enemy.GetFacingDirection() * _enemy.GetMoveSpeed(), _rb.velocity.y);
        if (_enemy.IsWallDetected() || !_enemy.IsGroundDetected())
        {
            _enemy.Flip();
            _stateMachine.ChangeState(_enemySkeleton.IdleState);
        }
    }
}
