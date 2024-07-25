using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    protected EnemySkeleton _enemySkeleton;

    public SkeletonAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName)
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
        _enemySkeleton.SetLastTimeAttacked();
    }

    public override void Update()
    {
        base.Update();
        _enemySkeleton.SetVelocityZero();

        if (_triggerCalled)
        {
            _stateMachine.ChangeState(_enemySkeleton.BattleState);
        }
    }
}