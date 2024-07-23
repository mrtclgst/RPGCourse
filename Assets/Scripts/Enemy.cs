using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Move Info")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _idleTimer;

    public EnemyStateMachine StateMachine { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.Update();
    }

    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }
    public float GetIdleTimer()
    {
        return _idleTimer;
    }
}