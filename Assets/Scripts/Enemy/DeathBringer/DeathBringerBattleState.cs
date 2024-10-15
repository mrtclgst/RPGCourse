using System.Collections;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    private EnemyDeathBringer _enemyDeathBringer;
    private Transform _playerTransform;
    private int _moveDir;

    public DeathBringerBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyDeathBringer enemyDeathBringer)
        : base(enemy, stateMachine, animBoolName)
    {
        _enemyDeathBringer = enemyDeathBringer;
    }

    public override void Enter()
    {
        base.Enter();
        _playerTransform = PlayerManager.Instance.Player.transform;

        //if (_playerTransform.GetComponent<PlayerStats>().IsDead())
        //_stateMachine.ChangeState(_enemyDeathBringer.MoveState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_enemy.IsPlayerDetected())
        {
            _stateTimer = _enemy.GetBattleTime();
            if (_enemy.IsPlayerDetected().distance < _enemy.GetAttackDistance() && CanAttack())
            {
                _stateMachine.ChangeState(_enemyDeathBringer.AttackState);
            }
            else
            {
                _stateMachine.ChangeState(_enemyDeathBringer.BattleState);
            }
        }

        if (_playerTransform.position.x > _enemy.transform.position.x)
        {
            _moveDir = 1;
        }
        else if (_playerTransform.position.x < _enemy.transform.position.x)
        {
            _moveDir = -1;
        }

        if (Vector2.Distance(_playerTransform.position, _enemyDeathBringer.transform.position) < _enemyDeathBringer.GetAttackDistance())
            return;

        _enemy.SetVelocity(_moveDir * _enemy.GetMoveSpeed(), _rb.velocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time > _enemy.GetAttackCooldown() + _enemy.GetLastTimeAttacked())
        {
            return true;
        }

        return false;
    }
}