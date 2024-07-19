using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine, string animBoolName)
        : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (_xInput == _player.GetFacingDirection() && _player.IsWallDetected())
        {
            return;
        }


        if (_xInput != 0 && !_player.IsBusy())
        {
            _stateMachine.ChangeState(_player.MoveState);
        }
    }
}