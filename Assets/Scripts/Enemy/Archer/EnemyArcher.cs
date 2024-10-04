using UnityEngine;

public class EnemyArcher : Enemy
{
    #region States
    public ArcherIdleState IdleState { get; private set; }
    public ArcherMoveState MoveState { get; private set; }
    public ArcherBattleState BattleState { get; private set; }
    public ArcherAttackState AttackState { get; private set; }
    public ArcherStunnedState StunnedState { get; private set; }
    public ArcherDeadState DeadState { get; private set; }
    public ArcherJumpState JumpState { get; private set; }
    #endregion

    [Header("Archer Specific Info")]
    [SerializeField] private GameObject _arrow;
    [SerializeField] private float _arrowSpeed;
    [SerializeField] private float _arrowDamage;


    [SerializeField] internal Vector2 _jumpVelocity;
    [SerializeField] internal float _jumpCooldown;
    [SerializeField] internal float _safetyDistance;
    internal float _lastTimeJumped;

    [Header("Additional Collision Check")]
    [SerializeField] private Transform _groundBehindCheck;
    [SerializeField] private Vector2 _groundBehindCheckSize;

    protected override void Awake()
    {
        base.Awake();
        IdleState = new(this, StateMachine, "Idle", this);
        MoveState = new(this, StateMachine, "Move", this);
        BattleState = new(this, StateMachine, "Idle", this);
        AttackState = new(this, StateMachine, "Attack", this);
        StunnedState = new(this, StateMachine, "Stunned", this);
        DeadState = new(this, StateMachine, "Idle", this);
        JumpState = new(this, StateMachine, "Jump", this);
    }
    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }
    internal override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            StateMachine.ChangeState(StunnedState);
            return true;
        }

        return false;
    }
    internal override void Die()
    {
        base.Die();
        StateMachine.ChangeState(DeadState);
    }
    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(_arrow, _attackCheckPoint.position, Quaternion.identity);
        newArrow.GetComponent<ArrowController>().SetupArrow(_arrowSpeed * _facingDirection, Stats);
    }
    public bool GroundBehindCheck()
    {
        return Physics2D.BoxCast(_groundBehindCheck.position, _groundBehindCheckSize, 0, Vector2.zero, 0, _whatIsGround);
    }
    public bool WallBehindCheck()
    {
        return Physics2D.Raycast(_wallCheck.position, Vector2.right * _facingDirection * -1, _wallCheckDistance + 2, _whatIsGround);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(_groundBehindCheck.position, _groundBehindCheckSize);
    }
}