using System.Collections;
using UnityEngine;

public class DeathBringerTeleportState : EnemyState
{
    private EnemyDeathBringer _enemyDeathBringer;
    public DeathBringerTeleportState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyDeathBringer enemyDeathBringer) : base(enemy, stateMachine, animBoolName)
    {
        _enemyDeathBringer = enemyDeathBringer;
    }
    public override void Enter()
    {
        base.Enter();
        _enemyDeathBringer.Stats.MakeInvincible(true);
    }
    public override void Update()
    {
        base.Update();

        if (_triggerCalled)
        {
            if (_enemyDeathBringer.CanCastSpell())
                _stateMachine.ChangeState(_enemyDeathBringer.SpellCastState);
            else
                _stateMachine.ChangeState(_enemyDeathBringer.BattleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        _enemy.Stats.MakeInvincible(false);
    }
}