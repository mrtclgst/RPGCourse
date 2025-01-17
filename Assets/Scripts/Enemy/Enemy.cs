using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    [Header("Collision Info")]
    [SerializeField] protected Transform _playerCheck;
    [SerializeField] protected LayerMask _whatIsPlayer;
    [SerializeField] protected float _playerCheckDistance;

    [Header("Move Info")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _idleTimer = 2;
    [SerializeField] private float _battleTime = 7;
    private float _defaultMoveSpeed;

    [Header("Attack Info")]
    [SerializeField] private float _attackDistance = 2;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private float _chaseDistance = 5;
    [SerializeField] private float _detectDistance = 10;
    private float _lastTimeAttacked;

    [Header("Stun Info")]
    [SerializeField] private Vector2 _stunForce = new(5, 6);
    [SerializeField] private float _stunDuration = 1;
    [SerializeField] protected bool _canBeStunned;
    [SerializeField] protected GameObject _counterImage;
    public EntityFX EntityFX { get; private set; }


    public EnemyStateMachine StateMachine { get; private set; }
    public string LastAnimBoolName { get; private set; }


    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
        _defaultMoveSpeed = _moveSpeed;
    }
    protected override void Start()
    {
        base.Start();
        _defaultMoveSpeed = _moveSpeed;
        EntityFX = GetComponent<EntityFX>();
    }
    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.Update();
    }

    #endregion

    #region GetMethods
    internal float GetMoveSpeed()
    {
        return _moveSpeed;
    }
    internal float GetIdleTimer()
    {
        return _idleTimer;
    }
    internal float GetAttackDistance()
    {
        return _attackDistance;
    }
    internal float GetLastTimeAttacked()
    {
        return _lastTimeAttacked;
    }
    internal float GetAttackCooldown()
    {
        return _attackCooldown;
    }
    internal float GetBattleTime()
    {
        return _battleTime;
    }
    internal float GetChaseDistance()
    {
        return _chaseDistance;
    }
    internal float GetDetectDistance()
    {
        return _detectDistance;
    }
    internal float GetStunDuration()
    {
        return _stunDuration;
    }
    internal Vector2 GetStunForce()
    {
        return _stunForce;
    }
    #endregion

    internal virtual void FreezeTimeFor(float duration)
    {
        StartCoroutine(IE_FreezeTimerFor(duration));
    }
    internal virtual void FreezeTime(bool timeFrozen)
    {
        if (timeFrozen)
        {
            _moveSpeed = 0;
            Animator.speed = 0;
        }
        else
        {
            Animator.speed = 1;
            _moveSpeed = _defaultMoveSpeed;
        }
    }
    internal virtual IEnumerator IE_FreezeTimerFor(float seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(seconds);
        FreezeTime(false);
    }
    public void SetLastTimeAttacked()
    {
        _lastTimeAttacked = Time.time;
    }
    public virtual RaycastHit2D IsPlayerDetected()
    {

        RaycastHit2D wallDetected =
            Physics2D.Raycast(_wallCheck.position, Vector2.right * _facingDirection, _playerCheckDistance, _whatIsGround);
        RaycastHit2D playerDetected =
            Physics2D.Raycast(_playerCheck.position, Vector2.right * _facingDirection, _playerCheckDistance, _whatIsPlayer);
        if (wallDetected)
        {
            if (wallDetected.distance < playerDetected.distance)
                return default(RaycastHit2D);
        }

        return playerDetected;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x + _attackDistance * _facingDirection, transform.position.y));
    }
    internal virtual bool CanBeStunned()
    {
        if (_canBeStunned)
        {
            return true;
        }
        return false;
    }
    public virtual void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }
    public virtual void AnimationSpecialAttackTrigger()
    {

    }

    public virtual void AssignLastAnimName(string animBoolName)
    {
        LastAnimBoolName = animBoolName;
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        _canBeStunned = true;
        _counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        _canBeStunned = false;
        _counterImage.SetActive(false);
    }
    #endregion

    public override void SlowEntityBy(float slowPercentage, float slowDuration)
    {
        _moveSpeed = _moveSpeed * (1 - slowPercentage);
        Animator.speed = Animator.speed * (1 - slowPercentage);
        Invoke("ReturnDefaultSpeed", slowDuration);
    }
    protected override void ReturnDefaultSpeed()
    {
        _moveSpeed = _defaultMoveSpeed;
        base.ReturnDefaultSpeed();
    }
    internal virtual void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }
}