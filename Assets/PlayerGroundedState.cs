using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
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

        if (!_player.IsGroundDetected())
            _stateMachine.ChangeState(_player.AirState);


        if (Input.GetKeyDown(KeyCode.Space) && _player.IsGroundDetected())
        {
            _stateMachine.ChangeState(_player.JumpState);
        }
    }
}