using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    protected EnemySkeleton _enemySkeleton;
    private Transform _playerTransform;
    private int _moveDir;
    private float _lastTimeFlipped;

    public SkeletonBattleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemySkeleton) : base(enemy, stateMachine, animBoolName)
    {
        _enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        _playerTransform = PlayerManager.Instance.Player.transform;
        _stateTimer = _enemy.GetBattleTime();
        _lastTimeFlipped = Time.time;

        if (_playerTransform.GetComponent<PlayerStats>().IsDead())
            _stateMachine.ChangeState(_enemySkeleton.MoveState);
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
                _stateMachine.ChangeState(_enemySkeleton.AttackState);
            }
        }
        else
        {
            if (_stateTimer < 0 || Vector2.Distance(_playerTransform.position, _enemy.transform.position) > _enemy.GetChaseDistance())
                _stateMachine.ChangeState(_enemySkeleton.IdleState);
        }


        //float distanceToPlayerX = Mathf.Abs(_playerTransform.position.x - _enemy.transform.position.x);
        //if (distanceToPlayerX < _enemy.GetAttackDistance() / 2f)
        //    return;


        int moveDir = 0;
        if (_playerTransform.position.x > _enemy.transform.position.x)
            moveDir = 1;
        else if (_playerTransform.position.x < _enemy.transform.position.x)
            moveDir = -1;
        if (_moveDir != moveDir && Time.time > _lastTimeFlipped + 0.3f)
        {
            _moveDir = moveDir;
            _lastTimeFlipped = Time.time;
            Debug.Log("here");
        }

        if (Vector2.Distance(_playerTransform.position, _enemySkeleton.transform.position) < _enemySkeleton.GetAttackDistance())
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
