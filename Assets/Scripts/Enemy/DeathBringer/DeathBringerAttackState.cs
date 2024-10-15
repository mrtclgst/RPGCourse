using System.Collections;
using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
    private EnemyDeathBringer _enemyDeathBringer;
    public DeathBringerAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyDeathBringer enemyDeathBringer)
        : base(enemy, stateMachine, animBoolName)
    {
        _enemyDeathBringer = enemyDeathBringer;
    }
    public override void Enter()
    {
        base.Enter();
        _enemyDeathBringer.IncreaseTeleportChance(5f);
    }

    public override void Exit()
    {
        base.Exit();
        _enemyDeathBringer.SetLastTimeAttacked();
    }

    public override void Update()
    {
        base.Update();
        _enemyDeathBringer.SetVelocityZero();

        if (_triggerCalled)
        {
            if (_enemyDeathBringer.CanTeleport())
            {
                _stateMachine.ChangeState(_enemyDeathBringer.TeleportState);
            }
            else
            {
                _stateMachine.ChangeState(_enemyDeathBringer.BattleState);
            }
        }
    }
}