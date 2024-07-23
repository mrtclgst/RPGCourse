using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) 
        : base(enemy, stateMachine, animBoolName, enemySkeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.GetIdleTimer();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (_stateTimer < 0f)
        {
            _stateMachine.ChangeState(_enemySkeleton.MoveState);
        }
    }
}
