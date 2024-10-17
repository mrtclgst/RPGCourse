using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SkillManager.Instance.GetSkillSword().DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        _player.StartCoroutine(_player.IE_BusyFor(0.2f));
    }

    public override void Update()
    {
        base.Update();

        _player.SetVelocityZero();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _stateMachine.ChangeState(_player.IdleState);
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x > _player.transform.position.x && _player.GetFacingDirection() != 1
            || mousePosition.x < _player.transform.position.x && _player.GetFacingDirection() != -1)
        {
            _player.Flip();
        }
    }
}
