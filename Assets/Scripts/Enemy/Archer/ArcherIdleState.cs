using System.Collections;
using UnityEngine;


public class ArcherIdleState : ArcherGroundedState
{
    public ArcherIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemyArcher)
        : base(enemy, stateMachine, animBoolName, enemyArcher)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.GetIdleTimer();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (_stateTimer < 0f)
        {
            _stateMachine.ChangeState(_enemyArcher.MoveState);
        }
    }
}
