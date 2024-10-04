using System.Collections;
using UnityEngine;

public class ArcherJumpState : EnemyState
{
    private EnemyArcher _enemyArcher;
    public ArcherJumpState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemyArcher)
        : base(enemy, stateMachine, animBoolName)
    {
        _enemyArcher = enemyArcher;
    }
    public override void Enter()
    {
        base.Enter();
        _rb.velocity = new Vector2(_enemyArcher._jumpVelocity.x * _enemy.GetFacingDirection() * -1, _enemyArcher._jumpVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        _enemy.Animator.SetFloat("yVelocity", _rb.velocity.y);
        if (_rb.velocity.y < 0 && _enemy.IsGroundDetected())
        {
            _stateMachine.ChangeState(_enemyArcher.BattleState);
        }
    }
}