using System.Collections;
using UnityEngine;

public class ArcherGroundedState : EnemyState
{
    protected Transform _player;
    protected EnemyArcher _enemyArcher;

    public ArcherGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemyArcher) : base(enemy, stateMachine, animBoolName)
    {
        _enemyArcher = enemyArcher;
    }

    public override void Enter()
    {
        base.Enter();
        _player = PlayerManager.Instance.Player.transform;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if (_enemyArcher.IsPlayerDetected() || Vector2.Distance(_enemy.transform.position, _player.transform.position) < _enemy.GetDetectDistance())
        {
            _stateMachine.ChangeState(_enemyArcher.BattleState);
        }
    }
}