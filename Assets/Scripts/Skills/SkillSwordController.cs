using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillSwordController : MonoBehaviour
{
    [SerializeField] private float _returnSpeed = 12;
    [SerializeField] private float _destroyDistanceBtwPlayer = 1f;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;
    private Player _player;
    private bool _canRotate = true;
    private bool _isReturning;

    [Header("Bounce Info")]
    [SerializeField] private float _bounceSpeedMultiplier = 1f;
    private bool _isBouncing;
    private int _bounceAmount = 0;
    private int _targetIndex = 0;
    private List<Transform> _targetList = new();

    [Header("Pierce Info")]
    private float _pierceAmount;

    [Header("Spin Info")]
    private float _maxTravelDistance;
    private float _spinDuration;
    private float _spinTimer;
    private bool _wasStopped;
    private bool _isSpinning;
    private float _spinDirection;


    private float _damageTimer;
    private float _damageCooldown;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
    }
    internal void SetupSword(Vector2 direction, float gravityScale, Player player)
    {
        _rigidbody.velocity = direction;
        _rigidbody.gravityScale = gravityScale;
        _player = player;

        if (_pierceAmount > 0)
            return;

        _spinDirection = Mathf.Clamp(_rigidbody.velocity.x, -1, 1);
        _animator.SetBool("Rotation", true);
    }
    internal void SetupBounce(bool canBounce, int bounceAmount)
    {
        _isBouncing = canBounce;
        _bounceAmount = bounceAmount;
    }
    internal void SetupPierce(int pierceAmount)
    {
        _pierceAmount = pierceAmount;
    }
    internal void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float damageCooldown)
    {
        _isSpinning = isSpinning;
        _spinDuration = spinDuration;
        _maxTravelDistance = maxTravelDistance;
        _damageCooldown = damageCooldown;
    }
    private void Update()
    {
        if (_canRotate)
        { transform.right = _rigidbody.velocity; }

        if (_isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _player.transform.position) < _destroyDistanceBtwPlayer)
            {
                _player.CatchSword();
            }
        }

        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (_isSpinning)
        {
            if (Vector2.Distance(_player.transform.position, transform.position) > _maxTravelDistance && !_wasStopped)
            {
                StopWhenSpinning();
            }

            if (_wasStopped)
            {
                _spinTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x + _spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                if (_spinTimer < 0)
                {
                    _isSpinning = false;
                    _isReturning = true;
                }
            }

            _damageTimer -= Time.deltaTime;
            if (_damageTimer < 0)
            {
                _damageTimer = _damageCooldown;
                Collider2D[] targetArray = Physics2D.OverlapCircleAll(transform.position, 1);
                foreach (Collider2D target in targetArray)
                {
                    target.GetComponent<Enemy>()?.TakeDamage();
                }
            }
        }
    }
    private void StopWhenSpinning()
    {
        _wasStopped = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;

        if (_spinTimer <= 0)
            _spinTimer = _spinDuration;
    }
    private void BounceLogic()
    {
        if (_isBouncing && _targetList.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetList[_targetIndex].position, _returnSpeed * _bounceSpeedMultiplier * Time.deltaTime);
            if (Vector2.Distance(transform.position, _targetList[_targetIndex].position) < 0.2f)
            {
                _targetList[_targetIndex].GetComponent<Enemy>()?.TakeDamage();
                _targetIndex++;
                _bounceAmount--;

                if (_bounceAmount <= 0)
                {
                    _isBouncing = false;
                    _isReturning = true;
                }

                if (_targetIndex >= _targetList.Count)
                    _targetIndex = 0;

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReturning)
            return;

        collision.GetComponent<Enemy>()?.TakeDamage();
        SetupTargetsForBouncing(collision);
        StuckInto(collision);
    }
    private void SetupTargetsForBouncing(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (_isBouncing && _targetList.Count <= 0)
            {
                Collider2D[] targetArray = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (Collider2D target in targetArray)
                {
                    if (target.GetComponent<Enemy>() != null)
                    {
                        _targetList.Add(target.transform);
                    }
                }
            }
        }
    }
    private void StuckInto(Collider2D collision)
    {
        if (_pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            _pierceAmount--;
            return;
        }
        if (_isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        _canRotate = false;
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        if (_isBouncing && _targetList.Count > 0)
            return;

        _animator.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
    public void ReturnSword()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        //_rigidbody.isKinematic = false;
        transform.parent = null;
        _isReturning = true;
    }
}