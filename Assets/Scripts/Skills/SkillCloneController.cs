using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCloneController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _attackCheckPoint;
    [SerializeField] private float _attackRadius;

    private float _duplicationChance;
    private float _cloneTimer;
    private SpriteRenderer _spriteRenderer;
    public Transform _closestEnemy;
    private bool _canDuplicate;
    private int _facingDir;
    private int _damage;

    [Space]
    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private float _closestEnemyCheckRadius = 25;

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
    public void SetupClone(Transform targetTransform, float cloneDuration, bool canAttack, Vector3 offset, Transform closestEnemy, bool canDuplicate, float duplicationChance, int damage)
    {
        transform.position = targetTransform.position + offset;
        _cloneTimer = cloneDuration;
        _closestEnemy = closestEnemy;
        _canDuplicate = canDuplicate;
        _duplicationChance = duplicationChance;
        _damage = damage;

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
                damageable.GetComponent<Entity>().SetKnockbackDirection(this.transform);
                damageable.GetComponent<Enemy>().TakeDamage(_damage, false);

                if (SkillManager.Instance.GetSkillClone().CanApplyOnHitEffect)
                {
                    ItemDataEquipment itemDataEquipment = Inventory.Instance.GetEquipment(EquipmentType.Weapon);
                    if (itemDataEquipment != null)
                        itemDataEquipment.ExecuteItemEffect(damageable.transform);
                }


                if (_canDuplicate)
                {
                    if (Random.Range(0, 100) < _duplicationChance)
                    {
                        SkillManager.Instance.GetSkillClone().CreateClone(damageable.transform, new Vector3(1f * _facingDir, 0));
                    }
                }
            }
        }
    }
    private void FaceClosestTarget()
    {
        if (_closestEnemy != null)
        {
            if (transform.position.x > _closestEnemy.transform.position.x)
            {
                _facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
