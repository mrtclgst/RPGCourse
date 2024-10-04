using System.Collections;
using UnityEngine;


public class ArcherStunnedState : EnemyState
{
    private EnemyArcher _enemyArcher;

    public ArcherStunnedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemyArcher)
        : base(enemy, stateMachine, animBoolName)
    {
        _enemyArcher = enemyArcher;
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.EntityFX.InvokeRepeating("RedColorBlink", 0, 0.1f);
        _stateTimer = _enemy.GetStunDuration();
        Vector2 stunForce = _enemy.GetStunForce();
        _rb.velocity = new Vector2(-1 * _enemy.GetFacingDirection() * stunForce.x, stunForce.y);
    }

    public override void Exit()
    {
        base.Exit();
        _enemy.EntityFX.Invoke("CancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();
        if (_stateTimer < 0)
        {
            _stateMachine.ChangeState(_enemyArcher.IdleState);
        }
    }
}