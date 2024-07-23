using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Info")]
    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected float _groundCheckDistance;
    [SerializeField] protected LayerMask _whatIsGround;
    [SerializeField] protected Transform _wallCheck;
    [SerializeField] protected float _wallCheckDistance;

    protected int _facingDirection = 1;
    protected bool _facingRight = true;

    #region Components
    public Animator Animator { get; private set; }
    public Rigidbody2D RB { get; private set; }
    #endregion

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        RB = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update()
    {

    }

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
        Gizmos.DrawLine(_wallCheck.position, new Vector2(_wallCheck.position.x + _wallCheckDistance, _wallCheck.position.y));
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
    }
    #endregion


    public void SetVelocity(float velocityX, float velocityY)
    {
        RB.velocity = new Vector2(velocityX, velocityY);
        FlipController(velocityX);
    }
    public void SetVelocityZero()
    {
        RB.velocity = new Vector2(0, 0);
    }

    #region GetFunctions
    public virtual int GetFacingDirection()
    {
        return _facingDirection;
    }
    #endregion
}