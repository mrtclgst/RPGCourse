using System.Collections;
using UnityEngine;

public class ArcherAttackState : EnemyState
{
    protected EnemyArcher _enemyArcher;

    public ArcherAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyArcher enemyArcher) : base(enemy, stateMachine, animBoolName)
    {
        _enemyArcher = enemyArcher;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        _enemyArcher.SetLastTimeAttacked();
    }

    public override void Update()
    {
        base.Update();
        _enemyArcher.SetVelocityZero();

        if (_triggerCalled)
        {
            _stateMachine.ChangeState(_enemyArcher.BattleState);
        }
    }
}