using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _stateTimer = _player.GetCounterAttackDuration();
        _player.Animator.SetBool("SuccessfulCounterAttack", false);
    }
    public override void Update()
    {
        base.Update();

        if (_player.TryToCounterAttack())
        {
            _stateTimer = 10;
            _player.Animator.SetBool("SuccessfulCounterAttack", true);
        }

        if (_stateTimer < 0 || _triggerCalled)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}