using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Details")]
    [SerializeField] private Vector2[] _attackMovementArray;
    [SerializeField] private float _counterAttackDuration;

    private bool _isBusy = false;

    [Header("Move Info")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _swordReturnForce;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDuration;
    private float _dashDirection = 1;
    private float _defaultMovementSpeed;
    private float _defaultJumpForce;
    private float _defaultDashSpeed;

    public PlayerFX PlayerFX { get; private set; }

    private GameObject _sword;

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
    public PlayerCounterAttackState CounterAttackState { get; private set; }
    public PlayerAimSwordState AimSwordState { get; private set; }
    public PlayerCatchSwordState CatchSwordState { get; private set; }
    public PlayerBlackholeState BlackholeState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    #endregion

    #region MonoBehaviours
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
        CounterAttackState = new PlayerCounterAttackState(this, StateMachine, "CounterAttack");
        AimSwordState = new PlayerAimSwordState(this, StateMachine, "AimSword");
        CatchSwordState = new PlayerCatchSwordState(this, StateMachine, "CatchSword");
        BlackholeState = new PlayerBlackholeState(this, StateMachine, "Jump");
        DeadState = new PlayerDeadState(this, StateMachine, "Die");
    }
    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
        _defaultMovementSpeed = _moveSpeed;
        _defaultJumpForce = _jumpForce;
        _defaultDashSpeed = _dashSpeed;
        PlayerFX = GetComponent<PlayerFX>();
    }
    protected override void Update()
    {
        if (Time.timeScale <= 0) return;

        base.Update();
        StateMachine.CurrentState.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F) && SkillManager.Instance.GetSkillCrystal().CrystalUnlocked)
        { SkillManager.Instance.GetSkillCrystal().UseSkill(); }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Inventory.Instance.UseFlask();
        }
    }
    #endregion
    public IEnumerator IE_BusyFor(float seconds)
    {
        _isBusy = true;
        yield return new WaitForSeconds(seconds);
        _isBusy = false;
    }
    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    private void CheckForDashInput()
    {
        if (!SkillManager.Instance.GetSkillDash().DashUnlocked)
            return;

        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.GetSkillDash().CanUseSkill())
        {
            SkillManager.Instance.GetSkillDash().UseSkill();
            _dashDirection = Input.GetAxisRaw("Horizontal");
            if (_dashDirection == 0)
                _dashDirection = _facingDirection;

            StateMachine.ChangeState(DashState);

        }
    }
    public override void DealDamage()
    {
        Collider2D[] damageableArray = Physics2D.OverlapCircleAll(_attackCheckPoint.position, _attackCheckRadius);
        foreach (var damageable in damageableArray)
        {
            if (damageable.GetComponent<Enemy>() != null)
            {
                Stats.DealDamage(damageable.GetComponent<EnemyStats>());

                Inventory.Instance.GetEquipment(EquipmentType.Weapon)?.ExecuteItemEffect(damageable.transform);
            }
        }
        base.DealDamage();
    }
    internal bool TryToCounterAttack()
    {
        bool hasEnemyCanBeStunned = false;

        Collider2D[] damageableArray = Physics2D.OverlapCircleAll(_attackCheckPoint.position, _attackCheckRadius);
        foreach (var damageable in damageableArray)
        {
            Enemy nearbyEnemy = damageable.GetComponent<Enemy>();
            if (nearbyEnemy != null)
            {
                if (nearbyEnemy.CanBeStunned())
                {
                    nearbyEnemy.CloseCounterAttackWindow();
                    hasEnemyCanBeStunned = true;
                    SkillManager.Instance.GetSkillParry().UseSkill();

                    if (CounterAttackState.GetCanCreateClone())
                    {
                        CounterAttackState.SetCanCreateClone(false);
                        //SkillManager.Instance.GetSkillClone().CreateCloneOnCounterAttack(nearbyEnemy.transform, 0.3f);
                        SkillManager.Instance.GetSkillParry().MakeMirageOnParry(nearbyEnemy.transform);
                    }
                }
            }
            else if (damageable.GetComponent<ArrowController>() != null)
            {
                damageable.GetComponent<ArrowController>().FlipArrow();
            }
        }

        return hasEnemyCanBeStunned;
    }
    public void AssignNewSword(GameObject newSword)
    {
        _sword = newSword;
    }
    public void CatchSword()
    {
        StateMachine.ChangeState(CatchSwordState);
        PlayerFX.PlayDustFX();
        Destroy(_sword);
    }
    internal override void Die()
    {
        StateMachine.ChangeState(DeadState);
        base.Die();
    }

    #region GetFunctions
    public float GetMoveSpeed()
    { return _moveSpeed; }
    public float GetJumpForce()
    { return _jumpForce; }
    public float GetDashSpeed()
    { return _dashSpeed; }
    public float GetDashDuration()
    { return _dashDuration; }
    public float GetDashDirection()
    { return _dashDirection; }
    public bool IsBusy()
    { return _isBusy; }
    public Vector2 GetAttackMovement(int comboCount)
    { return _attackMovementArray[comboCount]; }
    public float GetCounterAttackDuration()
    { return _counterAttackDuration; }
    public GameObject GetSword()
    { return _sword; }
    public float GetSwordReturnForce()
    { return _swordReturnForce; }
    #endregion

    public override void SlowEntityBy(float slowPercentage, float slowDuration)
    {
        _moveSpeed = _moveSpeed * (1 - slowPercentage);
        _jumpForce = _jumpForce * (1 - slowPercentage);
        _dashSpeed = _dashSpeed * (1 - slowPercentage);
        Animator.speed = Animator.speed * (1 - slowPercentage);
        Invoke("ReturnDefaultSpeed", slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        _moveSpeed = _defaultMovementSpeed;
        _jumpForce = _defaultJumpForce;
        _dashSpeed = _defaultDashSpeed;
        base.ReturnDefaultSpeed();
    }
}