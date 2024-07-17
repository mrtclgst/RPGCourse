using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _playerRb.velocity = new Vector2(_playerRb.velocity.x, _player.GetJumpForce());
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (_playerRb.velocity.y < 0)
        {
            _stateMachine.ChangeState(_player.AirState);
        }
    }
}