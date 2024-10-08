using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Info")]
    [SerializeField] protected Transform _attackCheckPoint;
    [SerializeField] protected float _attackCheckRadius = 1;
    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected float _groundCheckDistance = 1;
    [SerializeField] protected LayerMask _whatIsGround;
    [SerializeField] protected Transform _wallCheck;
    [SerializeField] protected float _wallCheckDistance = 1;

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 _knockbackForce = new(6, 12);
    [SerializeField] protected float _knockbackDuration = 0.1f;
    protected bool _isKnocked;

    public Action Entity_OnFlipped;

    protected int _facingDirection = 1;
    protected bool _facingRight = true;
    public int KnockbackDirection { get; private set; }

    #region Components
    public Animator Animator { get; private set; }
    public Rigidbody2D RB { get; private set; }
    //public EntityFX EntityFX { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public CharacterStats Stats { get; private set; }
    public CapsuleCollider2D Collider { get; private set; }
    #endregion

    #region MonoBehaviour
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
        //EntityFX = GetComponent<EntityFX>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Stats = GetComponent<CharacterStats>();
        Collider = GetComponent<CapsuleCollider2D>();
    }
    protected virtual void Update()
    {

    }
    #endregion

    #region Collisions
    public virtual bool IsGroundDetected()
    {
        if (Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _whatIsGround))
        {
            return true;
        }

        return false;
    }
    public virtual bool IsWallDetected()
    {
        if (Physics2D.Raycast(_wallCheck.position, Vector2.right * _facingDirection, _wallCheckDistance, _whatIsGround))
        {
            return true;
        }

        return false;
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(_groundCheck.position, new Vector2(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance));
        Gizmos.DrawLine(_wallCheck.position, new Vector2(_wallCheck.position.x + _wallCheckDistance * _facingDirection, _wallCheck.position.y));
        Gizmos.DrawWireSphere(_attackCheckPoint.position, _attackCheckRadius);
    }
    #endregion

    #region Flip
    protected virtual void FlipController(float velocityX)
    {
        if (velocityX < 0 && _facingRight || velocityX > 0 && !_facingRight)
        {
            Flip();
        }
    }
    public virtual void Flip()
    {
        _facingDirection = _facingDirection * -1;
        _facingRight = !_facingRight;
        transform.Rotate(0, 180, 0);
        Entity_OnFlipped?.Invoke();
    }
    public virtual void SetupDefaultDirection(int dir)
    {
        _facingDirection = dir;
        if (_facingDirection == -1)
        {
            _facingRight = false;
        }
    }
    #endregion

    public void SetVelocity(float velocityX, float velocityY)
    {
        if (_isKnocked)
            return;

        RB.velocity = new Vector2(velocityX, velocityY);
        FlipController(velocityX);
    }
    public void SetVelocityZero()
    {
        if (_isKnocked)
            return;

        RB.velocity = new Vector2(0, 0);
    }
    public virtual void SetKnockbackDirection(Transform damageSource)
    {
        if (damageSource.transform.position.x > transform.position.x)
        {
            KnockbackDirection = -1;
        }
        else if (damageSource.transform.position.x < transform.position.x)
        {
            KnockbackDirection = 1;
        }
    }
    #region GetFunctions
    public virtual int GetFacingDirection()
    {
        return _facingDirection;
    }
    #endregion

    public virtual void DealDamage()
    {
    }
    public virtual void TakeDamage(int damage, bool isShocked)
    {
        Stats.TakeDamage(damage, isShocked);
    }
    public void KnockbackEffect()
    {
        StartCoroutine(IE_HitKnockback());
    }
    protected virtual IEnumerator IE_HitKnockback()
    {
        _isKnocked = true;

        //if (_knockbackForce.magnitude > 0)
        RB.velocity = new Vector2(_knockbackForce.x * KnockbackDirection, _knockbackForce.y);
        yield return new WaitForSeconds(_knockbackDuration);
        _isKnocked = false;
    }
    public void MakeTransparent(bool transparency)
    {
        if (transparency)
        {
            SpriteRenderer.color = Color.clear;
        }
        else
        {
            SpriteRenderer.color = Color.white;
        }
    }
    internal virtual void Die()
    {

    }
    public virtual void SlowEntityBy(float slowPercentage, float slowDuration)
    {
    }
    protected virtual void ReturnDefaultSpeed()
    {
        Animator.speed = 1;
    }
}