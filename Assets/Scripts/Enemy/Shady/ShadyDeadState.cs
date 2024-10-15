using System.Collections;
using UnityEngine;

public class ShadyDeadState : EnemyState
{
    private EnemyShady _enemyShady;
    public ShadyDeadState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyShady enemyShady)
        : base(enemy, stateMachine, animBoolName)
    {
        _enemyShady = enemyShady;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (_triggerCalled)
        {
            _enemyShady.SelfDestroy();
        }
    }
}