using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected EnemySkeleton _enemySkeleton;
    protected Transform _playerTransform;

    public SkeletonGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName)
    {
        _enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        _playerTransform = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_enemySkeleton.IsPlayerDetected() || Vector2.Distance(_enemy.transform.position, _playerTransform.position) < _enemy.GetDetectDistance())
        {
            _stateMachine.ChangeState(_enemySkeleton.BattleState);
        }
    }
}
