using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity

{
    [Header("Collision Info")]
    [SerializeField] protected Transform _playerCheck;
    [SerializeField] protected LayerMask _whatIsPlayer;
    [SerializeField] protected float _playerCheckDistance;

    [Header("Move Info")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _idleTimer;

    [Header("Attack Info")]
    [SerializeField] private float _attackDistance;


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
    public float GetAttackDistance()
    {
        return _attackDistance;
    }


    public virtual RaycastHit2D IsPlayerDetected()
    {
        return Physics2D.Raycast(_playerCheck.position, Vector2.right * _facingDirection, _playerCheckDistance, _whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x + _attackDistance * _facingDirection, transform.position.y));
    }
}