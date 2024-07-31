using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCloneController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _attackCheckPoint;
    [SerializeField] private float _attackRadius;

    private float _cloneTimer;
    private SpriteRenderer _spriteRenderer;
    private Transform _closestEnemy;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        _cloneTimer -= Time.deltaTime;
        if (_cloneTimer < 0)
        {
            _spriteRenderer.color = new Color(1, 1, 1, _spriteRenderer.color.a - Time.deltaTime);
            if (_spriteRenderer.color.a < 0)
            { Destroy(gameObject); }
        }
    }
    public void SetupClone(Transform targetTransform, float cloneDuration, bool canAttack)
    {
        transform.position = targetTransform.position;
        _cloneTimer = cloneDuration;
        FaceClosestTarget();
        if (canAttack)
        {
            _animator.SetInteger("AttackNumber", Random.Range(1, 3));
        }
    }

    private void AnimationTrigger()
    {
        _cloneTimer = -1;
    }
    private void AttackTrigger()
    {
        Collider2D[] damageableArray = Physics2D.OverlapCircleAll(_attackCheckPoint.position, _attackRadius);
        foreach (var damageable in damageableArray)
        {
            if (damageable.GetComponent<Enemy>() != null)
            {
                damageable.GetComponent<Enemy>().TakeDamage();
            }
        }
    }

    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    _closestEnemy = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        if(_closestEnemy != null)
        {
            if(transform.position.x > _closestEnemy.transform.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
