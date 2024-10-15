using System.Collections;
using UnityEngine;


public class ShadyIdleState : ShadyGroundedState
{
    public ShadyIdleState(global::Enemy enemy, EnemyStateMachine stateMachine, string animBoolName, EnemyShady _enemyShady)
        : base(enemy, stateMachine, animBoolName, _enemyShady)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _enemy.GetIdleTimer();
    }

    public override void Update()
    {
        base.Update();
        if (_stateTimer < 0f)
        {
            _stateMachine.ChangeState(_enemyShady.MoveState);
        }
    }
}
