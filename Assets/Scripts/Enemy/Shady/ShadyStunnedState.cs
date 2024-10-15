using System.Collections;
using UnityEngine;


public class ShadyStunnedState : EnemyState
{
    private EnemyShady _enemyShady;
    public ShadyStunnedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyShady enemyShady)
        : base(enemy, stateMachine, animBoolName)
    {
        _enemyShady = enemyShady;
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
            _stateMachine.ChangeState(_enemyShady.IdleState);
        }
    }
}
