using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    protected EnemySkeleton _enemySkeleton;

    public SkeletonDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName)
    {
        _enemySkeleton = enemySkeleton;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.Animator.SetBool(_enemy.LastAnimBoolName, true);
        _enemy.Animator.speed = 0;
        _enemy.Collider.enabled = false;
        _stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (_stateTimer > 0)
        {
            _rb.velocity = new Vector2(0, 10);
        }
    }
}