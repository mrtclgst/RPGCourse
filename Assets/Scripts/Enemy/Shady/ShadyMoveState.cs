using System.Collections;
using UnityEngine;

public class ShadyMoveState : ShadyGroundedState
{
    public ShadyMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyShady enemyShady) : base(enemy, stateMachine, animBoolName, enemyShady)
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
            _stateMachine.ChangeState(_enemyShady.IdleState);
        }
    }
}