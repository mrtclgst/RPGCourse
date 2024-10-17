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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SkillBlackhole skillBlackhole = SkillManager.Instance.GetSkillBlackhole();
            if (skillBlackhole.BlackholeUnlocked && skillBlackhole.CanUseSkill())
            {
                SkillManager.Instance.GetSkillBlackhole().UseSkill();
                _stateMachine.ChangeState(_player.BlackholeState);
            }
        }

        if (Input.GetMouseButtonDown(1) && SkillManager.Instance.GetSkillParry().ParryUnlocked)
        { _stateMachine.ChangeState(_player.CounterAttackState); }

        if (!_player.IsGroundDetected())
        { _stateMachine.ChangeState(_player.AirState); }

        if (Input.GetMouseButtonDown(0))
        { _stateMachine.ChangeState(_player.PrimaryAttackState); }

        if (Input.GetKeyDown(KeyCode.Space) && _player.IsGroundDetected())
        { _stateMachine.ChangeState(_player.JumpState); }

        if (Input.GetKeyDown(KeyCode.Q) && _player.IsGroundDetected() && !HasSword() && SkillManager.Instance.GetSkillSword().SwordUnlocked)
        { _stateMachine.ChangeState(_player.AimSwordState); }
    }

    private bool HasSword()
    {
        if (_player.GetSword() != null)
        {
            _player.GetSword().GetComponent<SkillSwordController>().ReturnSword();
            return true;
        }

        return false;
    }
}