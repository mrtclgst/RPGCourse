using System.Collections;
using UnityEngine;

public class DeathBringerDeadState : EnemyState
{
    private EnemyDeathBringer _enemyDeathBringer;

    public DeathBringerDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyDeathBringer enemyDeathBringer) : base(enemy, stateMachine, animBoolName)
    {
        _enemyDeathBringer = enemyDeathBringer;
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.Animator.SetBool(_enemy.LastAnimBoolName, true);
        _enemy.Animator.speed = 0;
        _enemy.Collider.enabled = false;
        _stateTimer = 0.1f;
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