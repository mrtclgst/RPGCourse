using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    [SerializeField] private int _moveSpeed;
    [SerializeField] private int _jumpForce;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCooldown;
    private float _dashUsageTimer;
    private float _dashDirection = 1;
    private int _facingDirection = 1;
    private bool _facingRight = true;

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
    public PlayerDashState DashState { get; private set; }
    #endregion

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState = new PlayerAirState(this, StateMachine, "Jump");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
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
        CheckForDashInput();
    }

    public void SetVelocity(float velocityX, float velocityY)
    {
        PlayerRigidbody.velocity = new Vector2(velocityX, velocityY);
        FlipController(velocityX);
    }

    private void CheckForDashInput()
    {
        _dashUsageTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashUsageTimer < 0)
        {
            _dashUsageTimer = _dashCooldown;

            _dashDirection = Input.GetAxisRaw("Horizontal");
            if (_dashDirection == 0)
                _dashDirection = _facingDirection;

            StateMachine.ChangeState(DashState);

        }
    }
    private void FlipController(float velocityX)
    {
        if (velocityX < 0 && _facingRight || velocityX > 0 && !_facingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        _facingDirection = _facingDirection * -1;
        _facingRight = !_facingRight;
        transform.Rotate(0, 180, 0);
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
    public float GetDashSpeed() { return _dashSpeed; }
    public float GetDashDuration() { return _dashDuration; }
    public bool IsGroundDetected()
    {
        if (Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _whatIsGround))
        {
            return true;
        }

        return false;
    }
    public int GetFacingDirection()
    {
        return _facingDirection;
    }
    public float GetDashDirection()
    {
        return _dashDirection;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_groundCheck.position, new Vector2(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance));
        Gizmos.DrawLine(_wallCheck.position, new Vector2(_wallCheck.position.x + _wallCheckDistance, _wallCheck.position.y));
    }
}