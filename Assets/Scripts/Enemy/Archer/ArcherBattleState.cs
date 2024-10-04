using System.Collections;
using UnityEngine;


public class ArcherBattleState : EnemyState
{
    private EnemyArcher _enemyArcher;
    private int _moveDir;
    private Transform _playerTransform;

    public ArcherBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemyArcher) : base(enemy, stateMachine, animBoolName)
    {
        _enemyArcher = enemyArcher;
    }
    public override void Enter()
    {
        base.Enter();
        _playerTransform = PlayerManager.Instance.Player.transform;

        if (_playerTransform.GetComponent<PlayerStats>().IsDead())
            _stateMachine.ChangeState(_enemyArcher.MoveState);
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

            if (_enemyArcher.IsPlayerDetected().distance < _enemyArcher._safetyDistance)
            {
                if (CanJump())
                {
                    Jump();
                }
            }

            if (_enemy.IsPlayerDetected().distance < _enemy.GetAttackDistance() && CanAttack())
            {
                _stateMachine.ChangeState(_enemyArcher.AttackState);
            }
        }
        else
        {
            if (_stateTimer < 0 || Vector2.Distance(_playerTransform.position, _enemy.transform.position) > _enemy.GetChaseDistance())
                _stateMachine.ChangeState(_enemyArcher.IdleState);
        }

        if (_playerTransform.position.x > _enemy.transform.position.x && _enemy.GetFacingDirection() == -1)
        {
            _enemy.Flip();
        }
        else if (_playerTransform.position.x < _enemy.transform.position.x && _enemy.GetFacingDirection() == 1)
        {
            _enemy.Flip();
        }
    }

    private void Jump()
    {
        _stateMachine.ChangeState(_enemyArcher.JumpState);
        _enemyArcher._lastTimeJumped = Time.time;
    }

    private bool CanAttack()
    {
        if (Time.time > _enemy.GetAttackCooldown() + _enemy.GetLastTimeAttacked())
        {
            return true;
        }

        return false;
    }

    private bool CanJump()
    {
        if (_enemyArcher.GroundBehindCheck() == false || _enemyArcher.WallBehindCheck())
        {
            return false;
        }

        if (Time.time >= _enemyArcher._lastTimeJumped + _enemyArcher._jumpCooldown)
        {
            return true;
        }

        return false;
    }
}