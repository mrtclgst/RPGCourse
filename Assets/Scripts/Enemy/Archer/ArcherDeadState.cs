using System.Collections;
using UnityEngine;


public class ArcherDeadState : EnemyState
{
    protected EnemyArcher _enemyArcher;

    public ArcherDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemyArcher) : base(enemy, stateMachine, animBoolName)
    {
        _enemyArcher = enemyArcher;
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
