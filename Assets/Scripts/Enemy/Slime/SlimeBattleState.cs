using UnityEngine;

public class SlimeBattleState : EnemyState
{
    protected EnemySlime _enemySlime;
    private Transform _playerTransform;
    private int _moveDir;

    public SlimeBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySlime enemySlime) : base(enemy, stateMachine, animBoolName)
    {
        _enemySlime = enemySlime;
    }

    public override void Enter()
    {
        base.Enter();
        _playerTransform = PlayerManager.Instance.Player.transform;

        if (_playerTransform.GetComponent<PlayerStats>().IsDead())
            _stateMachine.ChangeState(_enemySlime.MoveState);
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
                _stateMachine.ChangeState(_enemySlime.AttackState);
            }
        }
        else
        {
            if (_stateTimer < 0 || Vector2.Distance(_playerTransform.position, _enemy.transform.position) > _enemy.GetChaseDistance())
            {
                _stateMachine.ChangeState(_enemySlime.IdleState);
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

        if (Vector2.Distance(_playerTransform.position, _enemySlime.transform.position) < _enemySlime.GetAttackDistance())
        {
            _enemy.SetVelocity(_moveDir * _enemy.GetMoveSpeed() / 3f, _rb.velocity.y);
        }
        else
        {
            _enemy.SetVelocity(_moveDir * _enemy.GetMoveSpeed(), _rb.velocity.y);
        }
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
