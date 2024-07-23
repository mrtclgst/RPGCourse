using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine, string animBoolName)
        : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        _player.SetVelocity(_xInput * _player.GetMoveSpeed(), _playerRb.velocity.y);

        if (_xInput == 0  || _player.IsWallDetected())
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }
}