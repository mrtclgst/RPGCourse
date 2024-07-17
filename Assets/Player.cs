using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    [SerializeField] private int _moveSpeed;
    [SerializeField] private int _jumpForce;

    [Header("Collision Info")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private float _wallCheckDistance;



    #region Components
    public Animator PlayerAnimator { get; private set; }
    public Rigidbody2D PlayerRigidbody { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    #endregion



    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState = new PlayerAirState(this, StateMachine, "Jump");
    }

    private void Start()
    {
        PlayerAnimator = GetComponentInChildren<Animator>();
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.Update();
    }

    public void SetVelocity(float velocityX, float velocityY)
    {
        PlayerRigidbody.velocity = new Vector2(velocityX, velocityY);
    }


    #region GetFunctions
    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }
    public float GetJumpForce()
    {
        return _jumpForce;
    }
    public bool IsGroundDetected()
    {
        if (Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _whatIsGround))
        {
            return true;
        }

        return false;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_groundCheck.position, new Vector2(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance));
        Gizmos.DrawLine(_wallCheck.position, new Vector2(_wallCheck.position.x + _wallCheckDistance, _wallCheck.position.y));
    }
}