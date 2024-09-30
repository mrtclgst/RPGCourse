using UnityEngine;

public class EnemySlime : Enemy
{
    [SerializeField] private SlimeType _slimeType;
    [SerializeField] private int _slimesToCreate;
    [SerializeField] private GameObject _slimePrefab;
    [SerializeField] private Vector2 _minSpawnVelocity;
    [SerializeField] private Vector2 _maxSpawnVelocity;

    #region States
    public SlimeIdleState IdleState { get; private set; }
    public SlimeMoveState MoveState { get; private set; }
    public SlimeBattleState BattleState { get; private set; }
    public SlimeAttackState AttackState { get; private set; }
    public SlimeStunnedState StunnedState { get; private set; }
    public SlimeDeadState DeadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        IdleState = new SlimeIdleState(this, StateMachine, "Idle", this);
        MoveState = new SlimeMoveState(this, StateMachine, "Move", this);
        BattleState = new SlimeBattleState(this, StateMachine, "Move", this);
        AttackState = new SlimeAttackState(this, StateMachine, "Attack", this);
        StunnedState = new SlimeStunnedState(this, StateMachine, "Stunned", this);
        DeadState = new SlimeDeadState(this, StateMachine, "Idle", this);
    }
    protected override void Start()
    {
        base.Start();
        SetupDefaultDirection(-1);
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


    protected override void Update()
    {
        base.Update();
    }
    internal override void Die()
    {
        base.Die();
        StateMachine.ChangeState(DeadState);

        if (_slimeType == SlimeType.Small) return;

        CreateSlimes();
    }

    private void CreateSlimes()
    {
        for (int i = 0; i < _slimesToCreate; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);
            newSlime.GetComponent<EnemySlime>().SetupSlime(_facingDirection);
        }
    }

    public void SetupSlime(int facingDir)
    {
        float xVelocity = Random.Range(_minSpawnVelocity.x, _maxSpawnVelocity.x);
        float yVelocity = Random.Range(_minSpawnVelocity.y, _maxSpawnVelocity.y);
        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -_facingDirection, yVelocity);
        _isKnocked = true;
        Invoke("CancelKnockback", 1.5f);
    }

    private void CancelKnockback()
    {
        _isKnocked = false;
    }
}

public enum SlimeType
{
    Small,
    Medium,
    Big
}