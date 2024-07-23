using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Details")]
    [SerializeField] Vector2[] _attackMovementArray;


    private bool _isBusy = false;

    [Header("Move Info")]
    [SerializeField] private int _moveSpeed;
    [SerializeField] private int _jumpForce;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCooldown;
    private float _dashUsageTimer;
    private float _dashDirection = 1;




    #region States
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }

    public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState = new PlayerAirState(this, StateMachine, "Jump");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, "Jump");
        PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.Update();
        CheckForDashInput();
    }
    public IEnumerator IE_BusyFor(float seconds)
    {
        _isBusy = true;
        yield return new WaitForSeconds(seconds);
        _isBusy = false;
    }
    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

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
    public float GetDashDirection()
    {
        return _dashDirection;
    }
    public bool IsBusy()
    {
        return _isBusy;
    }
    public Vector2 GetAttackMovement(int comboCount)
    {
        return _attackMovementArray[comboCount];
    }
    #endregion
}