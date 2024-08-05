using System.Collections;
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
    [SerializeField] private float _battleTime;
    private float _defaultMoveSpeed;

    [Header("Attack Info")]
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _detectDistance;
    private float _lastTimeAttacked;

    [Header("Stun Info")]
    [SerializeField] private Vector2 _stunForce;
    [SerializeField] private float _stunDuration;
    [SerializeField] protected bool _canBeStunned;
    [SerializeField] protected GameObject _counterImage;


    public EnemyStateMachine StateMachine { get; private set; }

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
        return Physics2D.Raycast(_playerCheck.position, Vector2.right * _facingDirection, _playerCheckDistance, _whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x + _attackDistance * _facingDirection, transform.position.y));
    }

    public virtual void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
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
    internal virtual bool CanBeStunned()
    {
        if (_canBeStunned)
        {
            return true;
        }

        return false;
    }
}