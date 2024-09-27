using UnityEngine;

public class SlimeGroundedState : EnemyState
{
    protected EnemySlime _enemySlime;
    protected Transform _playerTransform;

    public SlimeGroundedState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemySlime enemySlime) : base(enemy, stateMachine, animBoolName)
    {
        _enemySlime = enemySlime;
    }

    public override void Enter()
    {
        base.Enter();
        _playerTransform = PlayerManager.Instance.Player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_enemySlime.IsPlayerDetected() || Vector2.Distance(_enemy.transform.position, _playerTransform.position) < _enemy.GetDetectDistance())
        {
            _stateMachine.ChangeState(_enemySlime.BattleState);
        }
    }
}
