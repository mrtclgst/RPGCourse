using System.Collections;
using UnityEngine;

public class ShadyGroundedState : EnemyState
{

    protected Transform _player;
    protected EnemyShady _enemyShady;

    public ShadyGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyShady enemyShady) : base(enemy, stateMachine, animBoolName)
    {
        _enemyShady = enemyShady;
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
        if (_enemyShady.IsPlayerDetected() || Vector2.Distance(_enemy.transform.position, _player.transform.position) < _enemy.GetDetectDistance())
        {
            _stateMachine.ChangeState(_enemyShady.BattleState);
        }
    }
}
