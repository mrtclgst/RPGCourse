using System.Collections;
using UnityEngine;


public class ShadyBattleState : EnemyState
{
    private Transform _playerTransform;
    private EnemyShady _enemyShady;
    private int _moveDir;
    private float _defaultMoveSpeed;

    public ShadyBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyShady enemyShady)
        : base(enemy, stateMachine, animBoolName)
    {
        _enemyShady = enemyShady;
    }

    public override void Enter()
    {
        base.Enter();
        _playerTransform = PlayerManager.Instance.Player.transform;
        _defaultMoveSpeed = _enemy.GetMoveSpeed();
        _enemy.SetMoveSpeed(_enemyShady._battleMoveSpeed);

        if (_playerTransform.GetComponent<PlayerStats>().IsDead())
            _stateMachine.ChangeState(_enemyShady.MoveState);
    }

    public override void Update()
    {
        base.Update();

        if (_enemy.IsPlayerDetected())
        {
            _stateTimer = _enemy.GetBattleTime();
            if (_enemy.IsPlayerDetected().distance < _enemy.GetAttackDistance() && CanAttack())
            {
                //_stateMachine.ChangeState(_enemyShady.AttackState);
                //_stateMachine.ChangeState(_enemyShady.DeadState);
                _enemyShady.Stats.KillEntity();
            }
        }
        else
        {
            if (_stateTimer < 0 || Vector2.Distance(_playerTransform.position, _enemy.transform.position)
                > _enemy.GetChaseDistance())
            {
                Debug.Log(_stateTimer);
                _stateMachine.ChangeState(_enemyShady.IdleState);
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

        //if (Vector2.Distance(_playerTransform.position, _enemyShady.transform.position) < _enemyShady.GetAttackDistance())
        //{
        //    _enemy.SetVelocity(_moveDir * _enemy.GetMoveSpeed() / 3f, _rb.velocity.y);
        //}
        //else
        //{
        _enemy.SetVelocity(_moveDir * _enemy.GetMoveSpeed(), _rb.velocity.y);
        //}
    }

    public override void Exit()
    {
        base.Exit();
        _enemy.SetMoveSpeed(_defaultMoveSpeed);
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