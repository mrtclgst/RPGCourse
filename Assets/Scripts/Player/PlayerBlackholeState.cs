using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private float _flyTime = 0.3f;
    private bool _skillUsed;
    private float _defaultGravity;

    public PlayerBlackholeState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        _defaultGravity = _playerRb.gravityScale;
        _skillUsed = false;
        _stateTimer = _flyTime;
        _playerRb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        _playerRb.gravityScale = _defaultGravity;
        _player.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (_stateTimer > 0)
        {
            _playerRb.velocity = new Vector2(0, 15);
        }

        if (_stateTimer < 0)
        {
            _playerRb.velocity = new Vector2(0, -0.1f);
            //if (!_skillUsed)
            //{
            //    if (SkillManager.Instance.GetSkillBlackhole().CanUseSkill())
            //        SkillManager.Instance.GetSkillBlackhole().UseSkill();
            //    _skillUsed = true;
            //}
        }

        if (SkillManager.Instance.GetSkillBlackhole().BlackholeFinished())
        {
            _stateMachine.ChangeState(_player.AirState);
        }
    }
}