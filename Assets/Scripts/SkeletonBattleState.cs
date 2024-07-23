using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    protected EnemySkeleton _enemySkeleton;
    private Transform _playerTransform;
    private int _moveDir;

    public SkeletonBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName)
    {
        _enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        _playerTransform = _enemySkeleton.GetPlayerTransform();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_enemy.IsPlayerDetected())
        {
            if (_enemy.IsPlayerDetected().distance < _enemy.GetAttackDistance())
            {
                _enemy.SetVelocityZero();
                return;
            }
        }

        if (_playerTransform.position.x > _enemy.transform.position.x)
        {
            _moveDir = 1;
        }
        else if (_playerTransform.position.x < _enemy.transform.position.x)
        {
            _moveDir = -1;
        }

        _enemy.SetVelocity(_moveDir * _enemy.GetMoveSpeed(), _rb.velocity.y);
    }
}
