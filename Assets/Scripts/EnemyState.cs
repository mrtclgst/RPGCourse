using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine _stateMachine;
    protected Enemy _enemy;
    protected bool _triggerCalled;
    protected float _stateTimer;
    private string _animBoolName;
    protected Rigidbody2D _rb;

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName)
    {
        _enemy = enemy;
        _animBoolName = animBoolName;
        _stateMachine = stateMachine;
    }
    public virtual void Update()
    {
        _stateTimer -= Time.deltaTime;
    }
    public virtual void Enter()
    {
        _triggerCalled = false;
        _enemy.Animator.SetBool(_animBoolName, true);
        _rb = _enemy.RB;
    }
    public virtual void Exit()
    {
        _enemy.Animator.SetBool(_animBoolName, false);
    }
}