using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int _comboCounter;
    private float _lastTimeAttacked;
    private float _comboWindow = 2f;


    public PlayerPrimaryAttackState(Player player, PlayerStateMachine playerStateMachine, string animBoolName) : base(player, playerStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.Instance.PlaySFX(33, _player.transform);   //attack sfx

        if (_comboCounter > 2 || Time.time > _lastTimeAttacked + _comboWindow)
            _comboCounter = 0;

        _player.Animator.SetInteger("ComboCounter", _comboCounter);


        float attackDir = _player.GetFacingDirection();
        if (_xInput != 0)
            attackDir = _xInput;

        Vector2 attackMovement = _player.GetAttackMovement(_comboCounter);
        _player.SetVelocity(attackMovement.x * attackDir, attackMovement.y);


        _stateTimer = 0.05f;
    }

    public override void Exit()
    {
        base.Exit();

        _comboCounter++;
        _lastTimeAttacked = Time.time;
        _player.StartCoroutine(_player.IE_BusyFor(0.1f));
    }

    public override void Update()
    {
        base.Update();

        if (_stateTimer < 0)
        {
            _player.SetVelocityZero();
        }

        if (_triggerCalled)
        {
            _stateMachine.ChangeState(_player.IdleState);
        }
    }

    internal int GetComboCount()
    {
        return _comboCounter;
    }
}