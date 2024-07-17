using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _stateTimer = _player.GetDashDuration();
    }

    public override void Exit()
    {
        base.Exit();
        _player.SetVelocity(0, _playerRb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        _player.SetVelocity(_player.GetDashSpeed() * _player.GetDashDirection(), 0);
        if (_stateTimer < 0)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }
}