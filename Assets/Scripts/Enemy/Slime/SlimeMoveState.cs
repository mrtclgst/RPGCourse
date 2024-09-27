using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : SlimeGroundedState
{
    public SlimeMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySlime enemySlime)
        : base(enemy, stateMachine, animBoolName, enemySlime)
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
            _stateMachine.ChangeState(_enemySlime.IdleState);
        }
    }
}
