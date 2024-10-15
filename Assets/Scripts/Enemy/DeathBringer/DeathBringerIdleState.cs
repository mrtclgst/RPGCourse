using System.Collections;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private EnemyDeathBringer _enemyDeathBringer;
    private Transform _playerTransform;

    public DeathBringerIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyDeathBringer enemyDeathBringer) : base(enemy, stateMachine, animBoolName)
    {
        _enemyDeathBringer = enemyDeathBringer;
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.GetIdleTimer();

        if (_playerTransform == null)
            _playerTransform = PlayerManager.Instance.Player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(_playerTransform.position, _enemy.transform.position) < 10)
            _enemyDeathBringer.SetBossFightBegun(true);

        if (_stateTimer < 0 && _enemyDeathBringer.GetBossFightBegun())
        {
            _stateMachine.ChangeState(_enemyDeathBringer.BattleState);
        }
    }
}