using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine _stateMachine;
    protected Player _player;
    protected float _xInput;
    protected float _yInput;
    protected bool _triggerCalled;

    protected Rigidbody2D _playerRb;
    protected float _stateTimer;

    private string _animBoolName;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine, string animBoolName)
    {
        _player = player;
        _stateMachine = playerStateMachine;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        _playerRb = _player.RB;
        _player.Animator.SetBool(_animBoolName, true);
        _triggerCalled = false;
    }

    public virtual void Update()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");
        _player.Animator.SetFloat("yVelocity", _playerRb.velocity.y);
        _stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        _player.Animator.SetBool(_animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        _triggerCalled = true;
    }
}