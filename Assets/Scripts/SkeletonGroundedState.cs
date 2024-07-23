using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected EnemySkeleton _enemySkeleton;
    public SkeletonGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName)
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

        if (_enemySkeleton.IsPlayerDetected())
        {
            _stateMachine.ChangeState(_enemySkeleton.BattleState);
        }
    }
}
