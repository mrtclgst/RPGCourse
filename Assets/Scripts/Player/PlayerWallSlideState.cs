using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.ChangeState(_player.WallJumpState);
            return;
        }


        if (_xInput != 0 && _player.GetFacingDirection() != _xInput)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }

        if (_yInput < 0)
        {
            _player.SetVelocity(0, _playerRb.velocity.y);
        }
        else
        {
            _player.SetVelocity(0, _playerRb.velocity.y * 0.9f);
        }

        if (_player.IsGroundDetected())
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }
}